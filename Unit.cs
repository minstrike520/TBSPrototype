public enum OriginalAttribute
{
    OAtk,
    OSpd,
    OMHp,
}

public enum TunedAttribute
{
    Atk,
    Spd,
    MHp,
}

public enum State
{
    Hp,
}

public enum UnitEvent
{
    HealthRegen,
    Damaged,
    HealthCost,

}


public class Unit : ITurnObject
{
    public List<TurnCondition> TurnConditions;
    public List<ConstCondition> ConstConditions;
    public int OAtk { get; }
    public int OSpd { get; }
    public int OMHp { get; }
    public int Atk { get { return CalculateTunedAttribute(OAtk, GetTuners(TunedAttribute.Atk)); } }
    public int Spd { get { return CalculateTunedAttribute(OSpd, GetTuners(TunedAttribute.Spd)); } }
    public int Hp { get; set; }

    public Dictionary<OriginalAttribute, int> AttributeEnumValuePairs
    {
        get
        {
            return new Dictionary<OriginalAttribute, int>()
            {
                { OriginalAttribute.OAtk, this.OAtk },
                { OriginalAttribute.OSpd, this.OSpd },
                { OriginalAttribute.OMHp, this.OMHp },
            };
        }

    }
    public List<Tuner> GetTuners(TunedAttribute attribute)
    {
        List<Tuner> tuners = new();
        foreach (TurnCondition turnCondition in this.TurnConditions)
        {
            foreach (KeyValuePair<TunedAttribute, List<Tuner>> keyValuePair in turnCondition.Tuners)
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
    public void DoOperations()
    {
        foreach (TurnCondition turnCondition in this.TurnConditions)
        {
            foreach (KeyValuePair<State, List<Operation>> keyValuePair in turnCondition.Operations)
            {
                switch (keyValuePair.Key)
                {
                    case State.Hp:
                        foreach (Operation operation in keyValuePair.Value)
                            this.Hp = operation.Evaluate(this.Hp, this);
                        break;
                }
                
            }
        }
    }
    public static int CalculateTunedAttribute(int originalValue, List<Tuner> tuners)
    {
        int addend = 0;
        int multiplierPercentage = 0;
        foreach (Tuner tuner in tuners)
        {
            switch (tuner)
            {
                case FlatTuner:
                    addend += tuner.Value;
                    break;
                case MaxPercentageTuner:
                    multiplierPercentage += tuner.Value;
                    break;
            }
        }
        return (int)(originalValue * (1 + multiplierPercentage / 100.0f) + addend);
    }

    public Unit(int oAtk, int oSpd, int oHp)
    {
        this.OAtk = oAtk;
        this.OSpd = oSpd;
        this.OMHp = oHp;
        this.Hp = oHp;
        this.TurnConditions = new List<TurnCondition>();
        this.ConstConditions = new List<ConstCondition>();
    }
    void ITurnObject.NextTurn()
    {
        this.DoOperations();

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