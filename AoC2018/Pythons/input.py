import numpy
from myTypes import Map

class RawInput:
    def __init__(self, file, separator = "\n"):
        with open(file, "r") as f:
            self.raw = f.read()        
        self.parts = self.raw.strip().split(separator)
    
    def getRaw(self):
        return (self.raw+".")[:-1]

    def getParts(self):
        return self.parts.copy()

class Input(RawInput):
    def __init__(self, file, prepareFunc, separator = "\n"):
        super(Input, self).__init__(file, separator)
        self.prepared = []
        if (prepareFunc != None):
            for part in self.parts:
                self.prepared.append(prepareFunc(part.lstrip().rstrip()))

    def getInput(self):
        return self.prepared.copy()

class MapInput(RawInput):
    def __init__(self, file):
        super(MapInput, self).__init__(file)
        self.map = Map(self.raw)