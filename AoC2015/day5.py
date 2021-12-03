from input import *
import sys
import hashlib

class Word:
    def __init__(self, word):
        self.word = word

    def isNice(self):
        nougthies = ["ab", "cd", "pq", "xy"]
        for double in nougthies:
            if double in self.word:
                return 0
        
        vowels = "aeiou"
        vouelNum = 0
        for ch in vowels:
            vouelNum = vouelNum + self.word.count(ch)
        if vouelNum < 3:
            return 0
        return self.isRepeatWithIntercode(0)

    def isRepeatWithIntercode(self, intercode):
        i = 1+intercode
        while i < len(self.word):
            if self.word[i-1-intercode] == self.word[i]:
                return 1
            i = i+1
        return 0
    
    def twoLettersRepeat(self):
        i = 2
        while i < len(self.word):
            if self.word[i-2:i] in self.word[i:]:
                return 1
            i = i+1
        return 0

    def isNice2(self):
        if self.twoLettersRepeat() and self.isRepeatWithIntercode(1):
            return 1
        else:
            return 0

inp = Input(sys.argv[1], lambda x: Word(x))

nices = 0
nices2 = 0
for word in inp.prepared:
    nices = nices + word.isNice()
    nices2 = nices2 + word.isNice2()

print("part1: ", nices)
print("part2: ", nices2)