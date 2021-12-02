from input import Input
import sys

class QGroup:
    def __init__(self, cmd):
        self.questions = {}
        lines = cmd.split("\n")
        self.peopleCount = len(lines)
        for line in lines:
            line = line.strip()
            for ch in line:
                self.questions[ch] = self.questions.get(ch, 0) + 1
    
    def yesQuestions(self) -> int:
        return len(self.questions)

    def allYes(self) -> int:
        result = 0
        for k in self.questions:
            if self.questions[k] == self.peopleCount:
                result += 1
        return result

inp = Input(sys.argv[1], prepareFunc= lambda x: QGroup(x), separator="\n\n")

total = 0
allTotal = 0
for group in inp.prepared:
    total += group.yesQuestions()
    allTotal += group.allYes()

print("part1: ", total)
print("part2: ", allTotal)