public interface ITurnObject
{
    public void NextTurn()
    {
    }
}

public abstract class Tuner
{
    public int Value { get; set; }
}
public class TunerFlat : Tuner
{
    public TunerFlat(int value)
    {
        this.Value = value;
    }
}

public class TunerMaxPerc : Tuner
{
    public TunerMaxPerc(int percentage)
    {
        this.Value = percentage;
    }
}


public abstract class Operation
{

}


public abstract class Condition
{
    public string Name { get; set; }
    public Dictionary<Attribute, List<Tuner>> Tuners { get; set; }
    public Dictionary<State, List<Operation>> Operations { get; set; }
}

public class TurnConditionConsumed : Exception
{
    public TurnConditionConsumed()
    {

    }
}

public class TurnCondition : Condition, ITurnObject
{
    public int Turns { get; set; }
    void ITurnObject.NextTurn()
    {
        this.Turns -= 1;
        if (this.Turns == 0)
        {
            throw new TurnConditionConsumed();
        }
    }
}
public class ConstCondition : Condition
{

}
