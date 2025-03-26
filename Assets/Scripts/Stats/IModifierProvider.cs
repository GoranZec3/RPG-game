using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IModifierPrivider
    {
        //IEnumerator doesnt allow to go through in forloop
        // IEnumerator<float> GetAdditiveModifier(Stat stat);
        IEnumerable<float> GetAdditiveModifiers(Stat stat);

        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}