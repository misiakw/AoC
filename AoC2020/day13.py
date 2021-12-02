from input import RawInput
import sys
import time

inp = RawInput(sys.argv[1])
currentTime = int(inp.getParts()[0])
busLines = inp.getParts()[1].split(",")
numberedLines = [int(x) for x in inp.getParts()[1].replace("x,", "").split(",")]

timesToWait = [(x, x - currentTime%x) for x in numberedLines]
fastest = sorted(timesToWait, key=lambda x: x[1])[0]
print ("part1:", fastest[0]*fastest[1])

i = 0
delayDiffs = []
while i < len(busLines):
    if not busLines[i] == "x":
        delayDiffs.append((int(busLines[i]), i%int(busLines[i])))
    i+=1

def getCommon(a, b):
    print("match:", a, b, "inc", b[0])
    i = 1
    res = b[0]+b[1]
    while not res%a[0] == a[1]:
        res += b[0]
        i+=1
    return (a[0]*b[0], res) 

delayDiffs.sort(key=lambda x: x[0])

while len(delayDiffs) > 1:
    print("len", len(delayDiffs))
    newDif = []
    i = 0
    while i+1 < len(delayDiffs):
        newDif.append(getCommon(delayDiffs[i+1], delayDiffs[i]))
        i+= 2
    if i < len(delayDiffs):
        newDif.append(delayDiffs[i])
    delayDiffs = sorted(newDif, key=lambda x: x[0])
current = delayDiffs[0]
#for i in range(1, len(delayDiffs)):
#    print("check", i, "out of", len(delayDiffs), delayDiffs[i], current)
#    current = getCommon(current,delayDiffs[i])

print("task2:", current[0]-current[1])