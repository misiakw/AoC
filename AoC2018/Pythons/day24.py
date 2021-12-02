import sys
import math
from functools import cmp_to_key
from input import RawInput

inp = RawInput(sys.argv[1], "\n\n")

class Unit:
    def __init__(self, descr, num, armyName, boost):
        self.descr = descr
        self.selected = False
        self.id = num
        self.army = armyName
        self.boost = boost
        parts = descr.strip().split("with")
        self.size = int(parts[0].split(" ")[0])

        defParts = parts[1][:-2].strip().split("(")
        self.hp = int(defParts[0].split(" ")[0])
        self.weak = []
        self.immune = []
        if len(defParts) >= 2:
            typesParts = defParts[1].split(";")
            for typesSet in typesParts:
                for attack in typesSet.strip().split(","):
                    if "immune" in typesSet:
                        self.immune.append(attack.strip().split(" ")[-1])
                    if "weak" in typesSet:
                        self.weak.append(attack.strip().split(" ")[-1])

        attack = parts[2].strip().split()
        self.pwr = int(attack[4])
        self.attackType = attack[5]
        self.initiative = int(attack[9])

    def effectivePower(self)->int:
        return (self.pwr + self.boost) * self.size

    def __repr__(self)->str:
        return self.army+" group "+str(self.id)+"("+str(self.size)+"u, "+str(self.hp)+"hp, "+str(self.effectivePower())+"pwr)"

    def countDamageDealtTo(self, other)->int:
        if self.attackType in other.immune:
            return 0
        power = self.effectivePower()
        if self.attackType in other.weak:
            power *= 2
        return power

    def attack(self, target):
        if self.size <= 0:
            return 0
        killed = math.floor(self.countDamageDealtTo(target) / target.hp)
        if killed > target.size:
            killed = target.size
        target.size -= killed
        return killed
#        print(self.army, "group", self.id, "attacks defending group", target.id, "killing", killed, "units")

def attackOrderCompare(a, b):
    if a.effectivePower() == b.effectivePower():
        return a.initiative - b.initiative
    return a.effectivePower() - b.effectivePower()

def targetOrderCompare(a, b):
    #cel ataku max dmg -> max size -> max initiative
    if a[1] == b[1]:
        if a[2] == b[2]:
            return a[3]-b[3]
        return a[2]-b[2]
    return a[1]-b[1]


class Army:
    def __init__(self, units, name):
        self.units = units
        self.name = name

    def setBoost(self, boost):
        for unit in self.units:
            unit.boost = boost

    def __repr__(self):
        return str(self.units)
    
    def attackOrder(self):
        return sorted(self.units, key=cmp_to_key(attackOrderCompare), reverse=True)
    
    def getUnitById(self, identity):
        return next(filter(lambda x: x.id == identity, self.units))

    def isEmpty(self):
        for unit in filter(lambda x: x.size <= 0, self.units):
            self.units.remove(unit)  
        for unit in self.units:
            unit.selected = False

        return len(self.units) <= 0

    def getSize(self):
        res = 0
        for unit in self.units:
            res += unit.size
        return res

    def print(self):
        print(self.name)
        for unit in self.units:
            print("\tGroup", unit.id, "contains", unit.size, "units")

def GetArmy(descr, name, boost) -> Army:
    result = []
    i=1
    for line in descr.split("\n")[1:]:
        result.append(Unit(line, i, name, boost))
        i += 1
    return Army(result, name)

def GetAttackList(attacking, defending):
    result = []
    for att in attacking.attackOrder():
        possibleTargets = []
        for df in filter(lambda x: not x.selected, defending.units):
            dealtDmg = att.countDamageDealtTo(df)
            if dealtDmg > 0:
                possibleTargets.append((df, dealtDmg, df.effectivePower(), df.initiative))
#            print(attacking.name, "group", att.id, "would deal defending group",df.id, att.countDamageDealtTo(df), "damage")
        if len(possibleTargets) > 0:
            target = sorted(possibleTargets, key=cmp_to_key(targetOrderCompare), reverse=True)[0][0]
            target.selected = True
            result.append((att, target))
    return result

def getMiddle(a, b):
    return a+math.ceil((b-a)/2)

def Fight(Immune, Infection):
    while not Immune.isEmpty() and not Infection.isEmpty():
#        Immune.print()
#        Infection.print()
        attackList = GetAttackList(Infection, Immune)
        attackList2 = GetAttackList(Immune, Infection)
        attackList.extend(attackList2)

        killed = 0
        for unit in sorted(attackList, key=lambda x: x[0].initiative, reverse=True):
            killed += unit[0].attack(unit[1])
        if killed == 0:
            return None
    if Immune.isEmpty():
        return Infection
    return Immune

Immune = GetArmy(inp.parts[0], "Immune", 0)
Infection = GetArmy(inp.parts[1], "Infection", 0)

winArmy = Fight(Immune, Infection)

#first result = 14377
print("part1:", winArmy.getSize(), winArmy.name)

boost = 1
# part 1 - get mul boost
Immune = GetArmy(inp.parts[0], "Immune", boost)
Infection = GetArmy(inp.parts[1], "Infection", 0)
winArmy = Fight(Immune, Infection)

while winArmy.name == "Infection":
    boost *= 10
    Immune = GetArmy(inp.parts[0], "Immune", boost)
    Infection = GetArmy(inp.parts[1], "Infection", 0)
    winArmy = Fight(Immune, Infection)


isEnd = False
a = math.floor(boost/10)
b = boost
while not isEnd:
    mid = getMiddle(a, b)
    if mid == b:
        boost = mid
        isEnd = True
    Immune = GetArmy(inp.parts[0], "Immune", mid)
    Infection = GetArmy(inp.parts[1], "Infection", 0)
    winArmy = Fight(Immune, Infection)
    if winArmy is None:
        a += 1
        b += 1
    else:
        if winArmy.name == "Immune":
            b = mid
        else:
            a = mid

print("part2:", winArmy.getSize(), winArmy.units[0].boost)