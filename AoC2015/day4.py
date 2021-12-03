from input import *
import sys
import hashlib

inp = RawInput(sys.argv[1])

salt = 0
result = hashlib.md5((inp.raw+str(salt)).encode()).hexdigest()

while result[0:5] != "00000":
    salt = salt+1
    result = hashlib.md5((inp.raw+str(salt)).encode()).hexdigest()
print("result1:", salt)

while result[0:6] != "000000":
    salt = salt+1
    result = hashlib.md5((inp.raw+str(salt)).encode()).hexdigest()
print("result2:", salt)