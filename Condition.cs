using System.Collections.Generic;

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
public class FlatTuner : Tuner
{
    public FlatTuner(int value)
    {
        this.Value = value;
    }
}

public class MaxPercentageTuner : Tuner
{
    public MaxPercentageTuner(int percentage)
    {
        this.Value = percentage;
    }
}


public abstract class Operation
{
    public int Value { get; set; }
    public abstract int Evaluate(int originalValue, Unit unit);
}

public class FlatOperation: Operation
{
    public FlatOperation(int value)
    {
        this.Value = value;
    }
    public override int Evaluate(int originalValue, Unit unit)
    {
        return originalValue += this.Value;
    }
}

public class NowPercentageOperation : Operation
{
    public NowPercentageOperation(int percentage)
    {
        this.Value = percentage;
    }
    public override int Evaluate(int originalValue, Unit unit)
    {
        return (int)( originalValue * (1 + this.Value / 100.0f) );
    }
}

public class ByAttributeOperation : Operation
{
    public OriginalAttribute ReferringAttribute { get; }
    public ByAttributeOperation(int percentage, OriginalAttribute attribute)
    {
        this.Value = percentage;
        this.ReferringAttribute = attribute;
    }
    public override int Evaluate(int originalValue, Unit unit)
    {
        return (int)( originalValue + (unit.AttributeEnumValuePairs[this.ReferringAttribute] * this.Value / 100.0f) );
    }
}


public abstract class Condition
{
    public string Name { get; set; }
    public Dictionary<TunedAttribute, List<Tuner>> Tuners { get; set; }
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
