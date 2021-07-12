import argparse

parser = argparse.ArgumentParser()
parser.add_argument("-p", "--port", required=True)
args = parser.parse_args()
print(args.port)

# import sys
# print(sys.argv)