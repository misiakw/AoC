from input import Input
import sys

class BagsRule:
    def __init__(self, rule):
        sided = rule.split("contain")
        self.left = " ".join(sided[0].split(" ")[0:2])
        self.right = {}
        if "no other" in sided[1]:
            return
        for dst in sided[1].split(","):
            dstParts = dst.strip().split(" ")
            self.right[" ".join(dstParts[1:3])] = int(dstParts[0])

    def howManyContain(self):
        result = 0
        for key in self.right:
            result += self.right[key] *(bagDict[key].howManyContain() + 1)
        return result

    def __repr__(self):
        return self.left+" -> "+ str(self.right)

inp = Input(sys.argv[1], BagsRule)

bagDict = {}
for rule in inp.getInput():
    bagDict[rule.left] = rule\

canWrapGold = []
searchColor = ["shiny gold"]
while len(searchColor) > 0:
    newSearch = []
    for rule in inp.getInput():
        for color in searchColor:
            if rule.left not in canWrapGold and color in rule.right:
                canWrapGold.append(rule.left)
                newSearch.append(rule.left)
    searchColor = newSearch.copy()
    
print("part1: ", len(canWrapGold))
print("part2: ", bagDict["shiny gold"].howManyContain())