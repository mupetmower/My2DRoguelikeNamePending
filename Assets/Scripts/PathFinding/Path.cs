using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path {

    private ArrayList steps = new ArrayList();


    public Path()
    {

    }


    public int getLength()
    {
        return steps.Count;
    }


    public Step GetStepAt(int index)
    {
        return (Step)steps[index];
    }


    public int GetStepXAt(int index)
    {
        return GetStepAt(index).X;
    }


    public int GetStepYAt(int index)
    {
        return GetStepAt(index).Y;
    }


    public void AppendStep(int x, int y)
    {
        steps.Add(new Step(x, y));
    }


    public void PrependStep(int x, int y)
    {
        steps.Insert(0, new Step(x, y));
    }


    public bool Contains(int x, int y)
    {
        return steps.Contains(new Step(x, y));
    }


    public class Step
    {

        public int X { get; set; }
        public int Y { get; set; }


        public Step(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }

        override
        public bool Equals(object other)
        {
            if (other is Step) {
                Step o = (Step) other;

                return (o.X == X) && (o.Y == Y);
            }

            return false;
        }
    }
}
