from input import Input
import sys

class Product:
    def __init__(self, label):
        sides = label.split(" (contains ")
        self.allergens = sides[1][:-1].split(", ")
        self.ingredients = sides[0].split(" ")

inp = Input(sys.argv[1], lambda x: Product(x))
allergens = {}
ingredientCount = {}
products = inp.getInput()
#assign all possible ingredients to allergen
for product in products:
    for i in product.ingredients:
        ingredientCount[i] = ingredientCount.get(i, 0) +1
    for a in product.allergens:
        if allergens.get(a, None) == None:
            allergens[a] = product.ingredients.copy()
        else:
            [allergens[a].append(i) for i in product.ingredients if i not in allergens[a]]
#remove ingredient from possilble allergens if not on list
for product in products:
    for a in product.allergens:
        allergens[a] = [i for i in allergens[a] if i in product.ingredients]   

toClean = [allergens[x] for x in allergens if len(allergens[x])>1]
while len(toClean) > 0:
    for x in toClean:
        for c in [allergens[x][0] for x in allergens if len(allergens[x])==1]:
            if c in x:
                x.remove(c)
    toClean = [allergens[x] for x in allergens if len(allergens[x])>1]

alergic = [allergens[k][0] for k in allergens]
print("part1", sum([ingredientCount[k] for k in ingredientCount if k not in alergic]))
print("part2", ",".join([allergens[k][0] for k in sorted(allergens)]))