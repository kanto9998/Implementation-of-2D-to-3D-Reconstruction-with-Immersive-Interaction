from flask import Flask, jsonify

app = Flask("test")

@app.route('/data', methods=['GET'])
def get_data():
    data = {"message": "Hello from Python!"}
    return jsonify(data)

if __name__ == '__main__':
    app.run()