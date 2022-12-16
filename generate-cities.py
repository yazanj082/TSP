from random import *
NUMBER = 100;
def generateChars(number):
    if number<=25:
        return chr(number+65)
    if (number>25 and number<50):
        return chr(number+97-26)
    if number <=75:
        return 'a'+chr(number+65-50)
    return 'a'+chr(number+97-26-50)
graph = []
def generateDistances(number):
    tempStr=""
    temp=[]
    for i in range(0,number):
        temp.append(graph[i][number])
        tempStr+=str(graph[i][number])+","
    tempStr+="0"
    temp.append(0)
    randomNum=0
    for i in range(number+1,NUMBER):
        randomNum=randint(1, 100)
        temp.append(randomNum)
        tempStr+=","+str(randomNum)
    graph.append(temp)
    return tempStr
for i in range(NUMBER):
    print(generateChars(i)+" "+generateDistances(i))

