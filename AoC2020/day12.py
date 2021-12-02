from input import RawInput
from myTypes import Point
import sys

#  N
# W E
#  S

class Ship(Point):
    def __init__(self):
        self.x = 0
        self.y = 0
        self.facing = "E"
        self.directions = {"N":{"L":"W", "R":"E"}, "S":{"L":"E","R":"W"}, "E":{"L":"N","R":"S"}, "W":{"L":"S","R":"N"}}
        self.waypoint = Point(str(10)+","+str(-1))
    def rotate(self, dir, num):
        times = int(num/90)
        while times > 0:
            self.facing = self.directions[self.facing][dir]
            times -= 1
    def rotateWaypoint(self, dir, num):
        times = int(num/90)
        while times > 0:
            times -= 1
            tmp = self.waypoint.x
            if dir == "L":
                self.waypoint.x = self.waypoint.y
                self.waypoint.y = -1*tmp
            if dir == "R":
                self.waypoint.x = -1*self.waypoint.y
                self.waypoint.y = tmp
    def move(self, dir, num, point):
        if dir == "E":
            point.x += num
        if dir == "W":
            point.x -= num
        if dir == "N":
            point.y -= num
        if dir == "S":
            point.y += num
    def do(self, cmd):
        dir = cmd[0]
        num = int(cmd[1:])
        if dir in "LR":
            self.rotate(dir, num)
        if dir == "F":
            self.move(self.facing, num, self)
        if dir in "NSEW":
            self.move(dir, num, self)

    def do2(self, cmd):
        dir = cmd[0]
        num = int(cmd[1:])
        if dir in "NSEW":
            self.move(dir, num, self.waypoint)
        if dir in "LR":
            self.rotateWaypoint(dir, num)
        if dir == "F":
            self.x += num*self.waypoint.x
            self.y += num*self.waypoint.y
    def print(self):
        print("ship("+str(self)+", waypoint("+str(self.waypoint)+")")

inp = RawInput(sys.argv[1])
ship = Ship()
ship2 = Ship()
for cmd in inp.getParts():
    ship.do(cmd)
    ship2.do2(cmd)
print("part1", abs(ship.x)+abs(ship.y))
print("part2", abs(ship2.x)+abs(ship2.y))