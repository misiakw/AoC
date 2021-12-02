from input import Input
import sys

class TileFactory:
    def __init__(self):
        self.tiles = {}
        self.tiles[(0, 0)] = Tile(self, 0, 0)
    def getTileByPath(self, path):
        changes = {"ne":(-0.5, -1), "se":(-0.5, 1), "nw":(0.5, -1), "sw": (0.5, 1), "e": (-1, 0), "w":(1, 0)}
        x = 0.0
        y = 0.0
        for d in path:
            change = changes[d]
            x += change[0]
            y += change[1]
        if self.tiles.get((x, y), None) is None:
            self.tiles[(x, y)] = Tile(self, x, y)
        return self.tiles[(x, y)]
    def __getitem__(self, key):
        if self.tiles.get(key, None) is None:
            self.tiles[key] = Tile(self, key[0], key[1])
        return self.tiles[key]

class Tile:
    def __init__(self, factory, x, y):
        self.adjustment = []
        self.x = x
        self.y = y
        self.isWhite = True
        self.factory = factory
        self.futureState = True
    def flip(self):
        self.isWhite = not self.isWhite
    def populate(self):
        points = [(-0.5, -1), (-0.5, 1), (0.5, -1), (0.5, 1), (-1, 0), (1, 0)]
        for p in points:
            x = self.x + p[0]
            y = self.y + p[1]
            self.adjustment.append(self.factory[(x, y)])
    def prepareState(self):
        if len(self.adjustment) < 6:
            self.populate()
        blacks = len([t for t in self.adjustment if not t.isWhite])
        print(self.isWhite, blacks)
        if self.isWhite and blacks == 2:
            print("opa")
            self.futureState = False
            return
        if not self.isWhite and (blacks == 0 or blacks > 2):
            print("down")
            self.futureState = True
            return
        self.futureState = self.isWhite
    def updateState(self): self.isWhite = self.futureState

def parseInput(line):
    result = []
    i = 0
    while i < len(line):
        if line[i] == "s" or line[i] == "n":        
            result.append(line[i]+line[i+1])
            i+=1
        else:
            result.append(line[i])
        i+=1
    return result
inp = Input(sys.argv[1], parseInput)
factory = TileFactory()
for x in inp.getInput():
    t = factory.getTileByPath(x)
    t.flip()

print("part1", len([1 for k in factory.tiles if not factory.tiles[k].isWhite]))
tmp = [factory[k] for k in factory.tiles]

f2 = factory
print("day", 0, len([1 for k in f2.tiles if not factory.tiles[k].isWhite]))
for k in range(0, 2):
    tiles = [factory[k] for k in factory.tiles]
    [t.prepareState() for t in tiles]
    [t.updateState() for t in tiles]
    #print(factory.tiles)
    print("day", k+1, len([1 for k in factory.tiles if not factory.tiles[k].isWhite]))
print("part2", len([1 for k in factory.tiles if not factory.tiles[k].isWhite]))