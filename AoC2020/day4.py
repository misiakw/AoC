from input import Input
import sys

class Document:
    def __init__(self, raw):
        parts = raw.replace("\n", " ").split()
        self.document = {}
        for part in parts:
            kv = part.split(":")
            self.document[kv[0]] = kv[1]
    
    def __repr__(self):
        return self.document.__repr__()

    def isValid(self):
        if len(self.document) == 8:
            return 1
        if len(self.document) == 7 and self.document.get("cid") == None:
            return 1
        return 0

    def isValid2(self):
        if len(self.document) < 7:
            return 0
        if len(self.document) < 8 and self.document.get("cid") is not None:
            return 0

        byr = int(self.document.get("byr", "0"))
        if byr < 1920 or byr > 2002:
            return 0
        
        iyr = int(self.document.get("iyr", "0"))
        if iyr < 2010 or iyr > 2020:
            return 0

        eyr = int(self.document.get("eyr", "0"))
        if eyr < 2020 or eyr > 2030:
            return 0
        
        hUnit = self.document.get("hgt", "0cm")[-2:]
        hVal = int(self.document.get("hgt", "0cm")[:-2])
        if hUnit not in ["cm", "in"]:
            return 0
        if hUnit == "cm" and (hVal < 150 or hVal > 193):
            return 0
        if hUnit == "in" and (hVal < 59 or hVal > 76):
            return 0

        if self.document.get("ecl") not in ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"]:
            return 0

        hair = self.document.get("hcl")
        if len(hair) != 7 and hair[0] != "#":
            return 0
        try:
            int(hair[1:], 16)
        except:
            return 0

        passId = self.document.get("pid")
        if len(passId) != 9 or not passId.isnumeric():
            return 0

        return 1

inp = Input(sys.argv[1], prepareFunc= lambda x: Document(x), separator="\n\n")

valids = 0
valids2 = 0
for doc in inp.prepared:
    valids += doc.isValid()
    valids2 += doc.isValid2()

print("part1: ", valids)
print("part2: ", valids2)