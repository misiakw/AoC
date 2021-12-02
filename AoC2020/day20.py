from input import Input
import sys
import math
import numpy 

class Tile:
    def __init__(self, data):
        lines = data.split("\n")
        self.number = int(lines[0].split(" ")[1][:-1])
        self.size = len(lines[1])
        self.matches = []
        self.raw = numpy.chararray((self.size, self.size))
        for x in range(0, self.size):
            for y in range(0, self.size):
                self.raw[x, y] = lines[y+1][x]
    def getEdges(self):
        size = self.size
        edges = { "N":"", "N!":"","S":"", "S!":"","E":"", "E!":"","W":"", "W!":"" }
        for x in range(0, self.size):
            edges["N"] += self.raw[x, 0].decode("utf-8")
            edges["N!"] += self.raw[size-x-1, 0].decode("utf-8")
            edges["S"] += self.raw[size-x-1, size-1].decode("utf-8")
            edges["S!"] += self.raw[x, size-1].decode("utf-8")
        for y in range(0, size):
            edges["E"] += self.raw[size-1, y].decode("utf-8")
            edges["E!"] += self.raw[size-1, size-y-1].decode("utf-8")
            edges["W"] += self.raw[0, size-y-1].decode("utf-8")
            edges["W!"] += self.raw[0, y].decode("utf-8")
        return edges
    def __repr__(self): return "Tile<"+str(self.number)+">"
    def addMatch(self, other): 
        if other not in self.matches:
            self.matches.append(other)
    def getMatches(self):
        result = {}
        edges = self.getEdges()
        for k in edges:
            for m in self.matches:
                if edges[k] in m.getEdges().values():
                    result[k[0]] = m
        return result
    def applyPattern(self, pattern):
        pW = len(pattern[0])
        pH = len(pattern)
        result = False
        for y in range(0, self.size-pH):
            for x in range(0, self.size-pW):
                if self.patternFound(x, y, pattern):
                    result = True
        return result
    def patternFound(self, x, y, pattern)-> bool:
        for py in range(0, len(pattern)):
            for px in range(0, len(pattern[0])):
                if pattern[py][px] == "#":
                    if self.raw[y+py, x+px].decode("utf-8") == ".":
                        return False
        for py in range(0, len(pattern)):
            for px in range(0, len(pattern[0])):
                if pattern[py][px] == "#":
                    if self.raw[y+py, x+px].decode("utf-8") == "#":
                        self.raw[y+py, x+px] = "O".encode("utf-8")
    def rotate(self):
        self.raw = numpy.rot90(self.raw)
    def flip(self, axis):
        if axis == "X":
            self.raw = numpy.flip(self.raw, axis=1)
        else:
            self.raw = numpy.flip(self.raw, axis=0)
    def print(self):
        result = ""
        for y in range(0, self.size):
            result +="".join([self.raw[x, y].decode("utf-8") for x in range(0, self.size)])+"\n"
        return result
class Image:
    def __init__(self, corner, amountOfTiles):
        self.tileSize = corner.size
        self.size = int(math.sqrt(amountOfTiles))
        self.tiles = numpy.ndarray((self.size, self.size), dtype=Tile)

        self.tiles[0, 0] = corner
        for y in range(0, self.size):
            for x in range(0, self.size):
                self.fitTile(x, y)
    def __getitem__(self, key):
        tile = self.tiles[int(key[0]/self.tileSize), int(key[1]/self.tileSize)]
        if tile is None:
            return "+".encode("utf-8") if key[1]%self.tileSize == self.tileSize-1 or key[0]%self.tileSize == self.tileSize-1 else " ".encode("utf_8")
        return tile.raw[key[1]%self.tileSize, key[0]%self.tileSize]
    def fitTile(self, x, y):
        tile = self.tiles[y, x]        
        # x==0 y==0 rot zeby gora i lewa pusta
        if x==0 and y == 0:
            matches = corner.getMatches()
            while "S" not in matches or "E" not in matches:
                tile.rotate()
                matches = tile.getMatches()
        # x!=0 y==0 rot lewa ma pasowac, flip gora pusta
        if x!=0 and y==0:
            left = self.tiles[y, x-1]
            while left != tile.getMatches().get("W", None):
                tile.rotate()
            if "N" in tile.getMatches():
                tile.flip("X")
        # X==0 y!=0 rot gora ma pasowac, flip lewa pusta
        if x==0 and y!= 0:
            top = self.tiles[y-1, x]
            while top != tile.getMatches().get("N", None):
                tile.rotate()
            if "W" in tile.getMatches():
                tile.flip("Y")
        # x!=0 y!=0 rot lewa ma pasowac, flipgora ma pasowaz
        if x!=0 and y!=0:
            left = self.tiles[y, x-1]
            top = self.tiles[y-1, x]
            while left != tile.getMatches().get("W", None):
                tile.rotate()
            if top != tile.getMatches().get("N", None):
                tile.flip("X")

        if x < self.size-1:
            self.tiles[y, x+1] = tile.getMatches()["E"]
        if y < self.size-1:
            self.tiles[y+1, x] = tile.getMatches()["S"]
    def print(self, fullPic=True)->str:
        line = ""
        frames = [0, self.tileSize-1]
        for y in range(0, self.size*self.tileSize):
            modY = y%self.tileSize
            if fullPic and modY in frames:
                continue
            for x in range(0, self.size*self.tileSize):
                modX = x%self.tileSize
                if fullPic:
                    line += self[y, x].decode("utf-8") if modX not in frames else ""
                else:
                    line += self[y, x].decode("utf-8") if modX in frames or modY in frames else " "
            line+= "\n"
        return line

inp = Input(sys.argv[1], lambda x: Tile(x), separator="\n\n")
availableEdges = {}
tiles = {}
#match edges
for tile in inp.getInput():
    tiles[tile.number] = tile
    edges = tile.getEdges()
    for edge in [(edges[k], k) for k in edges]:
        if availableEdges.get(edge[0], None) is None:
            availableEdges[edge[0]] = (tile, edge[1])
        else:
            tile.addMatch( availableEdges[edge[0]][0])
            availableEdges[edge[0]][0].addMatch(tile)
            del availableEdges[edge[0]]

#reduce edges
resultTiles = {}
for k in availableEdges:
    num = availableEdges[k][0].number
    edge = availableEdges[k][1]
    if resultTiles.get(num, None) is None:
        resultTiles[num] = []
    if edge[0] not in resultTiles[num]:
        resultTiles[num].append(edge[0])

mul = numpy.prod([k for k in resultTiles if len(resultTiles[k]) == 2])
print("part1", mul)
corner = tiles[[k for k in resultTiles if len(resultTiles[k]) == 2][0]]
img = Image(corner, len(tiles))
fullTile = Tile("Tile 0:\n"+img.print(fullPic=True))
pattern = ["                  # ", "#    ##    ##    ###"," #  #  #  #  #  #   "]
i = 0
while not fullTile.applyPattern(pattern) and i < 8:
    if i%2 == 0:
        fullTile.flip("X")
    else:
        fullTile.flip("X")
        fullTile.rotate()
    i+=1
monster = fullTile.print()
suma = 0
for y in range(0, len(monster)):
    for x in range(0, len(monster[y])):
        if monster[y][x] == "#":
            suma += 1
print("part 2", suma)
#print(monster)