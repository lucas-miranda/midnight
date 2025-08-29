namespace Midnight.Test;

[TestClass]
public sealed class EntityFacts {
    private Scene _scene;
    private DummyPrototype _dummyProto;

    [TestInitialize]
    public void TestInitialize() {
        _scene = new();
        _scene.Begin();
        _dummyProto = new();
    }

    [TestMethod]
    public void TestValidateFieldsEmpty() {
        var e = new Entity();
        Assert.IsNull(e.Prototype);
        Assert.IsNotNull(e.Components);
    }

    [TestMethod]
    public void TestValidateFields() {
        var e = new Entity(1, null);
        Assert.IsNull(e.Prototype);
        Assert.IsNotNull(e.Components);
    }

    [TestMethod]
    public void TestValidateFieldsWithPrototype() {
        var e = _dummyProto.Instantiate();
        Assert.IsNotNull(e.Prototype);
        Assert.IsNotNull(e.Components);
    }

    [TestMethod]
    public void TestValidateFieldsWithPrototypeAndComponents() {
        var components = new Components();
        var e = new Entity(1, new DummyPrototype(), components);
        Assert.IsNotNull(e.Prototype);
        Assert.AreSame(e.Components, components);
    }

    [TestMethod]
    public void TestCopy() {
        var e1 = _dummyProto.Instantiate();
        var e2 = e1;
        Assert.AreSame(e1.Components, e2.Components);
        Assert.AreSame(e1.Prototype, e2.Prototype);
    }

    private class DummyPrototype : Prototype {
        protected override void Build() {
        }
    }
}
