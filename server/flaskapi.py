#!/usr/bin/env python
import io
import os
import base64
from PIL import Image
from flask import Flask, jsonify, request
from imutils import contours
import numpy as np
import imutils
import cv2

app = Flask(__name__)

ANSWER_KEY = {}
qc = 0
# test if server is running
@app.route('/')
def main():
   return "<h1 style='color:blue'>Hello API!</h1>"

#post method for config sending
@app.route('/api/config', methods=['POST'])
def config():
    config_string = request.data
    write_config(config_string)
    return request.data

#post method for snap sending
@app.route('/api/post', methods=['POST'])
def post_img():
    img_process(image_from_byte_array(request.data))
    return request.data

#get method for result returning
@app.route('/api/get', methods=['GET'])
def get_grade():
    if(os.path.isfile("result_image.bmp")):
        result_img = Image.open("result_image.bmp")
        new_width  = 700
        new_height = 900   
        result_img = result_img.resize((new_width, new_height), Image.ANTIALIAS)
        result_b64 = image_to_base64(result_img)
        return result_b64
    else:
        return str()

########### HELPER METHODS ##########
def write_config(config):
    f = open("config.cfg", "w")
    f.write(config)
    f.close()

def read_config():
    f = open("config.cfg", "r")
    config = f.readline()
    f.close()
    config = config.split("#")[:-1]
    for c in config:
        c = c.split(":")
        ANSWER_KEY[int(c[0])] = int(c[1])

def image_from_byte_array(byte_array):
    image = Image.open(io.BytesIO(byte_array))
    return image

def image_to_base64(image):
    buffer = io.BytesIO()
    image.save(buffer, format="BMP")
    img_str = base64.b64encode(buffer.getvalue())
    return img_str

# image processing and saving result as bmp to return it as base64 string
def img_process(img):
    if(os.path.isfile("result_image.bmp")):
        os.remove("result_image.bmp")

    read_config()
    qc = float(len(ANSWER_KEY))
    #image color convert
    image = cv2.cvtColor(np.array(img), cv2.COLOR_RGB2BGR)
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    thresh = cv2.threshold(gray, 0, 255,
        cv2.THRESH_BINARY_INV | cv2.THRESH_OTSU)[1]

    #finding contours
    cnts = cv2.findContours(thresh.copy(), cv2.RETR_EXTERNAL,
        cv2.CHAIN_APPROX_SIMPLE)
    cnts = cnts[0] if imutils.is_cv2() else cnts[1]
    questionCnts = []

    # loop over the contours and coumputing bounding box
    for c in cnts:
        (x, y, w, h) = cv2.boundingRect(c)
        ar = w / float(h)

        # check wide, tall, and aspect ratio for bounding to reject wrong boxes 
        if w >= 100 and h >= 75 and ar >= 1.25 and ar <= 1.5:
            questionCnts.append(c)

    questionCnts = contours.sort_contours(questionCnts, method="top-to-bottom")[0]
    correct = 0

    for (q, i) in enumerate(np.arange(0, len(questionCnts), 4)):
        # sort the contours for the current question from left to right
        cnts = contours.sort_contours(questionCnts[i:i + 4])[0]
        ract = None

        # loop over the sorted contours
        for (j, c) in enumerate(cnts):
            # construct a mask that reveals only the current
            mask = np.zeros(thresh.shape, dtype="uint8")
            cv2.drawContours(mask, [c], -1, 255, -1)
            # apply the mask to the thresholded image, then
            # count the number of non-zero pixels in the
            mask = cv2.bitwise_and(thresh, thresh, mask=mask)
            total = cv2.countNonZero(mask)
            # if the current total has a larger number of total
            # non-zero pixels, then we are examining the currently
            # ract-in answer
            if ract is None or total > ract[0]:
                ract = (total, j)
        # initialize the contour color and the index of the
        # *correct* answer
        color = (0, 0, 255)
        k = ANSWER_KEY[q]
        # check to see if the ract answer is correct
        if k == ract[1]:
            color = (0, 255, 0)
            correct += 1
        # draw the outline of the correct answer on the test
        cv2.drawContours(image, [cnts[k]], -1, color, 3)
    # grab the result
    score = (correct / qc) * 100
    cv2.putText(image, "{:.2f}%".format(score), (200, 200),
        cv2.FONT_HERSHEY_SIMPLEX, 3, (0, 0, 255), 3)
    cv2.imwrite("result_image.bmp", image)

if __name__ == '__main__':
    #app.run(host='0.0.0.0', port='8080')
    app.run(host='192.168.0.157', port='8080')
