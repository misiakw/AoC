import numpy

class RawInput:
    def __init__(self, file):
        with open(file, "r") as f:
            self.raw = f.read()
    
    def getRaw(self):
        return (self.raw+".")[:-1]
    
    def getLines(self):
        return self.raw.strip().splitlines()

class Input(RawInput):
    def __init__(self, file, prepareFunc):
        super(Input, self).__init__(file)
        self.lines = self.raw.strip().splitlines()
        self.prepared = []
        if (prepareFunc != None):
            for line in self.lines:
                self.prepared.append(prepareFunc(line.lstrip().rstrip()))

    def getInput(self):
        return self.prepared.copy()