import sys
import math
from functools import cmp_to_key
from input import Input
class Point3d:
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z
    def dist(self, other):
        return abs(self.x - other.x) + abs(self.y - other.y) + abs(self.z - other.z)

class Range3d:
    def __init__(self, smallest, highest):
        self.smallest = smallest
        self.highest = highest
    def getSize(self):
        dx = abs(self.highest.x - self.smallest.x)
        dy = abs(self.highest.y - self.smallest.y)
        dz = abs(self.highest.z - self.smallest.z)
        return dx*dy*dz
class Nanobot(Point3d):
    def __init__(self, descr):
        self.sharing = []
        self.size = 0
        parts = descr.split(" ")
        loc = parts[0][5:-2].split(",")
        super(Nanobot, self).__init__(int(loc[0]), int(loc[1]), int(loc[2]))
        self.r = int(parts[1][2:])
    def __repr__(self):
        return "pos=<"+str(self.x)+","+str(self.y)+","+str(self.y)+"> r="+str(self.r)+" sharing "+str(len(self.sharing))
   
    def shareRange(self, bot):
        result = self.dist(bot) <= (self.r+bot.r)
        if result:
            self.sharing.append(bot)
        self.size = len(self.sharing)
        return result

    def getPoint(self, descr)->Point3d:
        x = self.x
        y = self.y
        z = self.z

        x = self.x-self.r if descr[0] == "l" else x
        x = self.x+self.r if descr[0] == "r" else x
        y = self.y-self.r if descr[1] == "h" else y
        y = self.y+self.r if descr[1] == "l" else y
        z = self.z-self.r if descr[2] == "b" else z
        z = self.z+self.r if descr[2] == "f" else z
        return Point3d(x, y, z)

    def getTiles(self):
        tiles = []
        #upperFloor
        tiles.append(Range3d(self.getPoint("lhb"), self.getPoint("ccc")))
        tiles.append(Range3d(self.getPoint("chb"), self.getPoint("rcc")))
        tiles.append(Range3d(self.getPoint("lhc"), self.getPoint("ccf")))
        tiles.append(Range3d(self.getPoint("chc"), self.getPoint("rfc")))
        #lowerFloor
        tiles.append(Range3d(self.getPoint("lcb"), self.getPoint("clc")))
        tiles.append(Range3d(self.getPoint("lcc"), self.getPoint("clf")))
        tiles.append(Range3d(self.getPoint("ccb"), self.getPoint("rlc")))
        tiles.append(Range3d(self.getPoint("ccc"), self.getPoint("rlf")))
        return tiles

def nanobotCompare(a, b):
    return a.r - b.r if a.size == b.size else b.size - a.size

def checkTile(point, dx, dy, dz):
    print(point, dx, dy, dz)

inp = Input(sys.argv[1], lambda  x: Nanobot(x))
nanobots = inp.getInput()

strongest = sorted(nanobots, key=lambda x: x.r, reverse=True)[0]
count = 0
for nanobot in nanobots:
    for secondBot in nanobots:
        if secondBot is not nanobot:
            secondBot.shareRange(nanobot)
    if nanobot == strongest:
        next
    if strongest.dist(nanobot) <= strongest.r:
        count += 1
print("part1:", count)
nanobots.sort(key=cmp_to_key(nanobotCompare))
tiles = nanobots[0].getTiles()
print(tiles)
print(tiles[0].getSize())