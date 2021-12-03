from input import *
import sys

class Box:
    def __init__(self, size):
        self.sizes = []
        self.pows = None
        for x in size.strip().split('x'):
            self.sizes.append(int(x))
        self.sizes.sort()

    def getSidePows(self):
        if (self.pows == None):
            self.pows = [(self.sizes[0]*self.sizes[1]), (self.sizes[1]*self.sizes[2]), (self.sizes[2]*self.sizes[0])]
        return self.pows
    
    def getPow(self):
        return sum(self.getSidePows()) * 2
    
    def getOverlay(self):
        return min(self.getSidePows())

    def getTie(self):
        return self.sizes[0]*self.sizes[1]*self.sizes[2]

    def getWrap(self):
        return 2*(sum(self.sizes[0:2]))

def mapToBox(size):
    return Box(size)

inp = Input(sys.argv[1], mapToBox)

paperSize = 0
ribbonLen = 0
for box in inp.prepared:
    paperSize = paperSize + box.getPow() + box.getOverlay()
    ribbonLen = ribbonLen + box.getWrap() + box.getTie()

print("part1: "+str(paperSize))
print("part2: "+str(ribbonLen))