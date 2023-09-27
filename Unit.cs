public enum Attribute
{
    atk,
    spd,
}

public enum State
{

}
public class Unit : ITurnObject
{
    public List<TurnCondition> TurnConditions;
    public List<ConstCondition> ConstConditions;
    public int OAtk { get; }
    public int OSpd { get; }
    public List<Tuner> GetTuners(Attribute attribute)
    {
        List<Tuner> tuners = new();
        foreach (TurnCondition turnCondition in this.TurnConditions)
        {
            foreach (KeyValuePair<Attribute, List<Tuner>> keyValuePair in turnCondition.Tuners)
            {
                if (keyValuePair.Key == attribute)
                {
                    foreach (Tuner tuner in keyValuePair.Value)
                        tuners.Add(tuner);
                }
            }
        }
        return tuners;
    }
    public static int CalculateTunedAttribute(int originalValue, List<Tuner> tuners)
    {
        int addend = 0;
        int multiplierPercentage = 0;
        foreach (Tuner tuner in tuners)
        {
            switch (tuner)
            {
                case TunerFlat:
                    addend += tuner.Value;
                    break;
                case TunerMaxPerc:
                    multiplierPercentage += tuner.Value;
                    break;
            }
        }
        return (int)(originalValue * (1 + multiplierPercentage / 100.0f) + addend);
    }

    public int GetAtk()
    {
        return CalculateTunedAttribute(OAtk, GetTuners(Attribute.atk));
    }
    public int GetSpd()
    {
        return CalculateTunedAttribute(OSpd, GetTuners(Attribute.spd));
    }

    public Unit(int oAtk, int oSpd)
    {
        this.OAtk = oAtk;
        this.OSpd = oSpd;
        this.TurnConditions = new List<TurnCondition>();
        this.ConstConditions = new List<ConstCondition>();
    }
    void ITurnObject.NextTurn()
    {
        List<TurnCondition> turnConditionsWaitingToRemove = new List<TurnCondition>();
        foreach (ITurnObject turnCondition in this.TurnConditions)
        {
            try
            {
                turnCondition.NextTurn();
            }
            catch (TurnConditionConsumed)
            {
                turnConditionsWaitingToRemove.Add((TurnCondition)turnCondition);
            }
        }
        foreach (TurnCondition turnCondition in turnConditionsWaitingToRemove)
        {
            this.TurnConditions.Remove(turnCondition);
        }
    }
}