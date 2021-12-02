from input import RawInput
import sys

class Circle:
    def __init__(self, content):
        self.circle = []
        for ch in content:
            self.circle.append(int(ch))
        self.current = 0
    def __getitem__(self, key)->int:
        if isinstance(key, slice):
            end = key.stop if key.stop >= 0 else key.start + len(self.circle) + key.stop
            return [self[i] for i in range(key.start, end)]
        k = int(key)
        return self.circle[(self.current+k)%len(self.circle)]
    def shuffle(self):
        picked = self[self.current+1: self.current+4]
        rest = self[self.current+4:-3]
        toSelect = self.circle[self.current]-1
        while toSelect in picked or toSelect == 0:
            toSelect -= 1
            if toSelect < min(self.circle):
                toSelect = max(rest)
        x = 0
        while rest[x] != toSelect:
            x+=1
        currentVal = self[self.current]
        self.circle = rest[:x+1] + picked + rest[x+1:]
        x = 0
        while self[x] != currentVal:
            x += 1
        self.current = (x+1)%len(self.circle)
        
    def __repr__(self):
        result = []
        for i in range(0, len(self.circle)):
            result.append("("+str(self.circle[i])+")" if i == self.current else str(self.circle[i]))
        return " ".join(result)


inp = RawInput(sys.argv[1])
c = Circle(inp.getRaw())
for i in range(0, 100):
    c.shuffle()
s = 0
while c[s] != 1:
    s+=1
print("part1:", "".join([str(c[s+x]) for x in range(1, len(c.circle))]))
 
