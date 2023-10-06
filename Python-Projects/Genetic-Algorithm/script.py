"""Genetic algorithm used for star wars project"""

import random
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

SOURCE = "./Data/position_sample.csv"

df = pd.read_csv(SOURCE, header=0, delimiter=";")


class Individual:
    """un Individual est constitué d'une liste de parametres (p1,p2,p3,p4,p5,p6)
    et de l'erreur [moyenne d'erreurs X et Y, erreur X, erreur Y]"""

    def __init__(self, param: list = None):
        """_summary_

        Args:
            param (list, optional): List of the 6 parameters. Defaults to None.
        """
        if param is None:  # If parameters are not given, random ones are chosen
            list_p = []
            for p_idx in range(6):
                if p_idx in (0, 3):
                    list_p.append(random.uniform(-100, 100))
                elif p_idx in (1, 4):
                    list_p.append(random.uniform(0, 100))
                else:
                    list_p.append(random.uniform(0, 2 * np.pi))
            self.params = list_p
        else:
            self.params = param

        self.error = Individual.fitness(self)

    def __str__(self) -> str:
        """String version of an Individual (its parameters and its errors)

        Returns:
            str: the string that you can print
        """
        return str(self.params) + " " + str(self.error)

    def fitness(self) -> tuple:
        """Determine for each point of the dataset the disance between accurate x (resp. y)
        and predicted x (resp. predicted y)

        Returns:
            tuple: (mean error, x error, y error)
        """
        dist_x = []
        dist_y = []

        for df_idx in range(df.shape[0]):
            time = df["#t"].iloc[df_idx]

            x_predicted = self.params[0] * np.sin(
                (self.params[1] * time) + self.params[2]
            )
            xdelta = abs(df["x"].iloc[df_idx] - x_predicted)

            y_predicted = self.params[3] * np.sin(
                (self.params[4] * time) + self.params[5]
            )
            ydelta = abs(df["y"].iloc[df_idx] - y_predicted)

            dist_x.append(xdelta)
            dist_y.append(ydelta)

        # To evaluate our performance we only consider the max error for x and y
        max_x = max(dist_x)
        max_y = max(dist_y)
        return np.mean([max_x, max_y]), max_x, max_y


def create_rand_pop(count: int) -> list:
    """Create a population (list of Individuals)

    Args:
        count (int): Amount of Individuals in the pop

    Returns:
        list: the list of individuals
    """
    return [Individual() for _ in range(count)]


def mutation(params: list, error: float, var: str) -> Individual:
    """Mutate an individual (slightly change parameters)

    Args:
        params (list): list of parameters
        error (float): the error from this individual
        var (str): indicate if we are mutating x parameters or y ones

    Returns:
        Individual: the mutated individual
    """
    newval = []
    val = error * 0.95
    epsilon = [random.uniform(-val, val) for z in range(3)]
    y_add = 3 if var.upper() == "Y" else 0

    newval.append(max(min(params[0 + y_add] + epsilon[0], 100), -100))
    newval.append(max(min(params[1 + y_add] + epsilon[1], 100), 0))
    newval.append(max(min(params[2 + y_add] + epsilon[2], 2 * np.pi), 0))

    if var.upper() == "X":
        newval += params[3:]
    elif var.upper() == "Y":
        newval = params[:3] + newval
    else:
        print("Error")
    return Individual(newval)


def evaluate(pop: list, var: str) -> list:
    """Sort the list of individuals in a ascending way
    according to x error or y error

    Args:
        pop (list): the list of individuals
        var (str): indicate if we are evaluating x error or y error

    Returns:
        list: the sorted list
    """
    eval_idx = 1 if var.upper() == "X" else 2
    pop.sort(key=lambda a: a.error[eval_idx])
    return pop


def selection(pop: list, rate: int) -> tuple:
    """Get the first (rate/2)% and the last ones of the list

    Args:
        pop (list): the list of individuals (population)
        rate (int): full rate

    Returns:
        tuple: The two parts we selected
    """
    nbval = int(len(pop) * (rate / 2))
    return pop[0:nbval], pop[len(pop) - nbval :]


def crossing(ind1: Individual, ind2: Individual) -> tuple:
    """Crossing two individuals [p1A,p2B,p3A,...] and [p1B,p2A,p3B,...]

    Args:
        ind1 (Individual):
        ind2 (Individual):

    Returns:
        Tuple: The two crossed individuals
    """
    ls1 = zip(ind1.params[0::2], ind2.params[-1::-2])
    ls2 = zip(ind2.params[0::2], ind1.params[-1::-2])

    ls1 = [item for sublist in ls1 for item in sublist]
    ls2 = [item for sublist in ls2 for item in sublist]

    cross1, cross2 = Individual(ls1), Individual(ls2)
    return cross1, cross2


def crossing_bis(ind1_bis: Individual, ind2_bis: Individual) -> tuple:
    """Alternative version of crossing() method. It is used to cross y parameters
    without crossing x ones

    Args:
        ind1 (Individual):
        ind2 (Individual):

    Returns:
        tuple: The two crossed individuals
    """
    # [p1X, p2X, p3X, p4A, P5B, P6B] and [p1X, p2X, p3X, p4B, p5A, p6A]
    ls1, ls2 = (
        ind1_bis.params[0:4] + ind2_bis.params[4:],
        ind2_bis.params[0:4] + ind1_bis.params[4:],
    )
    cross1, cross2 = Individual(ls1), Individual(ls2)
    return cross1, cross2


# Initialisation des paramètres :
N_POP = 100
EPS = 3
SELECT_RATE = 0.3  # Will be divided by 2
CROSS_RATE = 0.1  # Will be multiplied by 2
STEP_X = 0
STEP_Y = 0

# For graphical purpose
listX = []
list_genX = []
listY = []
list_genY = []

lsTest = create_rand_pop(N_POP)
lsTest = evaluate(lsTest, "X")

# We first focus on x parameters (p1, p2, p3)
while (
    lsTest[0].error[1] >= EPS
):  # it means "while the minimal x error is greater than EPS"
    # Selection (15 Individuals)
    start, end = selection(lsTest, SELECT_RATE)

    # Crossing (20 Individuals)
    cross = []
    for i in np.arange(int(N_POP * CROSS_RATE)):
        idx2, idx1 = crossing(start[i], end[i])
        cross.append(idx1)
        cross.append(idx2)

    # Mutation (65 Individuals)
    mut_pop = []
    for i in range(65):
        mut_idx = random.randint(0, len(lsTest) - 1)
        ls_p = lsTest[mut_idx].params
        mut = mutation(ls_p, lsTest[mut_idx].error[1], "X")
        mut_pop.append(mut)

    lsTest = start + cross + mut_pop
    lsTest = evaluate(lsTest, "X")

    STEP_X += 1

    # Every time we reduced the max error, we keep the generation
    # and the value to plot them later + we print them
    if not listX or lsTest[0].error[1] < listX[-1]:
        list_genX.append(STEP_X)
        listX.append(lsTest[0].error[1])
        print("Generation :", STEP_X)
        print("X error (max) :", lsTest[0].error[1])

print("Best X parameters found")
solX = lsTest[0].params

# Création d'une nouvelle population avec (p1,p2,p3) fixé
manip1 = solX.copy()
y_error_init = lsTest[0].error[2]

lsTest = []
for i in range(N_POP):
    element = mutation(manip1, y_error_init, "Y")
    lsTest.append(element)
lsTest = evaluate(lsTest, "Y")

# We can now focus on y parameters (p4, p5, p6) with x parameters fixed
while lsTest[0].error[2] >= (EPS * 3):
    start, end = selection(lsTest, SELECT_RATE)

    cross = []
    for i in np.arange(int(N_POP * CROSS_RATE)):
        idx2, idx1 = crossing_bis(start[i], end[i])
        cross.append(idx1)
        cross.append(idx2)

    mut_pop = []
    for i in range(65):
        mut_idx = random.randint(0, len(lsTest) - 1)
        ls_p = lsTest[mut_idx].params
        mut = mutation(ls_p, lsTest[mut_idx].error[2], "Y")
        mut_pop.append(mut)

    lsTest = start + cross + mut_pop
    lsTest = evaluate(lsTest, "Y")

    STEP_Y += 1

    if not listY or lsTest[0].error[2] < listY[-1]:
        list_genY.append(STEP_Y)
        listY.append(lsTest[0].error[2])
        print("Generation :", STEP_Y)
        print("y error (max) :", lsTest[0].error[2])
        print("mean error (max) :", lsTest[0].error[0])

STEP_TOT = STEP_X + STEP_Y

print("END\n", lsTest[0])
print("Number of generations :", STEP_TOT)

plt.step(list_genX, listX, color="r")
plt.step(list_genY, listY, color="b")
plt.title("Evolution of x and y errors through generation")
plt.legend(["X", "Y"])
plt.show()
