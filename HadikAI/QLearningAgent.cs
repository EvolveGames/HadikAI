using System;
using System.Collections.Generic;

namespace ReinforcementLearningModels;
public class QLearningAgent<TState, TAction>
{
    private Dictionary<string, Dictionary<TAction, double>> QTable; // Q-tabule pro každý stav
    private double learningRate;
    private double discountFactor;
    private double explorationRate;
    private double minExplorationRate;
    private double explorationDecay;

    public QLearningAgent(double learningRate, double discountFactor, double initialExplorationRate, double minExplorationRate, double explorationDecay)
    {
        this.learningRate = learningRate;
        this.discountFactor = discountFactor;
        this.explorationRate = initialExplorationRate;
        this.minExplorationRate = minExplorationRate;
        this.explorationDecay = explorationDecay;
        QTable = new Dictionary<string, Dictionary<TAction, double>>();
    }

    // Vytvoření jedinečného klíče pro stav
    private string GetStateKey(TState state)
    {
        return state.ToString(); // Nebo jiný způsob, jak reprezentovat stav
    }

    public TAction GetAction(TState state, List<TAction> actions)
    {
        string stateKey = GetStateKey(state);

        // Zkontroluj, jestli existuje Q-tabule pro daný stav
        if (!QTable.ContainsKey(stateKey))
        {
            QTable[stateKey] = new Dictionary<TAction, double>();
            foreach (var action in actions)
            {
                QTable[stateKey][action] = 0.0; // Inicializuj Q-values na 0
            }
        }

        // Epsilon-greedy strategie
        if (new Random().NextDouble() < explorationRate)
        {
            return actions[new Random().Next(actions.Count)]; // Náhodná akce
        }
        else
        {
            TAction bestAction = actions[0];
            double maxQValue = double.MinValue;

            foreach (var action in actions)
            {
                if (QTable[stateKey][action] > maxQValue)
                {
                    maxQValue = QTable[stateKey][action];
                    bestAction = action;
                }
            }
            return bestAction; // Nejlepší akce na základě Q-values
        }
    }

    public void UpdateQValue(TState state, TAction action, double reward, TState nextState, List<TAction> actions)
    {
        string stateKey = GetStateKey(state);
        string nextStateKey = GetStateKey(nextState);

        // Inicializace Q-tabule pro následující stav
        if (!QTable.ContainsKey(nextStateKey))
        {
            QTable[nextStateKey] = new Dictionary<TAction, double>();
            foreach (var a in actions)
            {
                QTable[nextStateKey][a] = 0.0; // Inicializuj Q-values na 0
            }
        }

        // Aktualizace Q-value
        double oldQValue = QTable[stateKey][action];
        double maxNextQValue = double.MinValue;

        foreach (var a in actions)
        {
            if (QTable[nextStateKey][a] > maxNextQValue)
            {
                maxNextQValue = QTable[nextStateKey][a];
            }
        }

        // Q-learning update rule
        double newQValue = oldQValue + learningRate * (reward + discountFactor * maxNextQValue - oldQValue);
        QTable[stateKey][action] = newQValue;
    }

    public void UpdateExplorationRate()
    {
        explorationRate = Math.Max(minExplorationRate, explorationRate * explorationDecay);
    }
}
