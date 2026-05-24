using Moq;
using static Movement;
using static Phase;
using static Troop;

public class TestUnit
{
    Unit unit;
    public TestUnit(Stats stats, Captain captain, List<Mock<TroopPerk>> perkList)
    {
        this.unit.stats = stats;
        this.unit.captain = captain;
        this.unit.perkList = new List<UnitPerk>();
        foreach( Mock<TroopPerk> perk in perkList)
        {
            Mock<UnitPerk> unitPerk = new Mock<UnitPerk>();
            unitPerk.Setup(p => p.InstantiatePerk(It.IsAny<TroopContext>())).Returns(perk.As<TroopPerk>().Object);
            this.unit.perkList.Add(unitPerk.Object);
        }
    }

    public TestUnit(Stats stats, Captain captain, List<TroopPerk> perkList)
    {
        this.unit.stats = stats;
        this.unit.captain = captain;
        this.unit.perkList = new List<UnitPerk>();
        foreach( TroopPerk perk in perkList)
        {
            Mock<UnitPerk> unitPerk = new Mock<UnitPerk>();
            unitPerk.Setup(p => p.InstantiatePerk(It.IsAny<TroopContext>())).Returns(perk);
            this.unit.perkList.Add(unitPerk.Object);
        }
    }

    public Unit GetUnit()
    {
        return unit;
    }
}
