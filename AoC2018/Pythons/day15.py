from input import MapInput
import sys
from functools import cmp_to_key

class Creature:
    def __init__(self, x, y, chart, name="Creature"):
        self.x = x
        self.y = y
        self.name = name
        self.reach = {}
        self.chart = chart

    def move(self, creatures):
        targets = filter(lambda x: x.name is not self.name, creatures)
        reachable = []
        for t in targets:
            for r in filter(lambda x: self.reach.get(x, 0), t.getRange()):
                reachable.append(r)
        reachable.sort(key=cmp_to_key(lambda x, y: self.reach[x] - self.reach[y]))
        minMove = self.reach[reachable[0]]
        nearest = filter(lambda x: self.reach[x] == minMove, reachable)
        selected = sorted(nearest, key=cmp_to_key(orderCompare))[0]
        #selected position

        newMap = self.chart.copy()
        newMap[selected] = "@"
        print(newMap)
        return

    def reset(self, chart):
        self.chart = chart
        self.reach = {}
        self.reach[(self.x, self.y)] = 0

        self.reachTo((self.x-1, self.y), 1)
        self.reachTo((self.x, self.y-1), 1)
        self.reachTo((self.x+1, self.y), 1)
        self.reachTo((self.x, self.y+1), 1)

        result = {}
        for p  in filter(lambda x: self.reach[x] > 0, self.reach):
            result[p] = self.reach[p]
        self.reach = result

    def reachTo(self, pkt, dist):
        if self.chart[pkt] != ".":
            self.reach[pkt] = -1
            return
        if self.reach.get(pkt, 9999999999) < dist:
            return
        self.reach[pkt] = dist
        self.reachTo((pkt[0]-1, pkt[1]), dist+1)
        self.reachTo((pkt[0], pkt[1]-1), dist+1)
        self.reachTo((pkt[0]+1, pkt[1]), dist+1)
        self.reachTo((pkt[0], pkt[1]+1), dist+1)

    def getRange(self):
        pts = [(self.x-1, self.y), (self.x, self.y-1), (self.x+1, self.y), (self.x, self.y+1)]
        result = []
        for p in pts:
            isValidX = p[0] >= 0 and p[0] < self.chart.width
            isValidY = p[1] >= 0 and p[1] < self.chart.height
            if isValidX and isValidY and self.chart[p] == ".":
                result.append(p)
        return result

    def attack(self, creatures):
        return

    def __repr__(self):
        return self.name+"("+str(self.x)+","+str(self.y)+")"

def creatureOrderCompare(a, b):
    if a.y == b.y:
        return a.x - b.x
    return a.y - b.y
def orderCompare(a, b):
    if a[1] == b[1]:
        return a[0] - b[0]
    return a[1] - b[1]

inp = MapInput(sys.argv[1])

chart = inp.map
creatures = []
for y in range(0, chart.height):
    for x in range(0, chart.width):
        if chart[x,y] == "G":
            creatures.append(Creature(x, y, chart, "goblin"))        
        if chart[x,y] == "E":
            creatures.append(Creature(x, y, chart, "elf"))
creatures.sort(key=cmp_to_key(creatureOrderCompare))

runs = 1
while runs:
    print(chart)
    print(creatures)
    for creature in filter(lambda x: x.name=="elf", creatures):
        creature.reset(chart.copy())
        creature.move(creatures)
        creature.attack(creatures)
    runs = 0
