using azure_proto_core;
using NUnit.Framework;
using System;

namespace azure_proto_core_test
{
    public class ResourceTypeTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CompareToZeroResourceType()
        {
            ResourceType resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test/providers/Microsoft.Web/sites/autoreport");
            ResourceType resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test/providers/Microsoft.Web/sites/autoreport");
            Assert.AreEqual(0, resourceType1.CompareTo(resourceType2));

            resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            Assert.AreEqual(0, resourceType1.CompareTo(resourceType2));

            ResourceType resourceType3 = ResourceType.None;
            ResourceType resourceType4 = ResourceType.None;
            Assert.AreEqual(0, resourceType3.CompareTo(resourceType4));
        }

        [Test]
        public void CompareToOneResourceType()
        {
            ResourceType resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test/providers/Microsoft.Web/sites/autoreport");
            ResourceType resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101");
            Assert.AreEqual(1, resourceType1.CompareTo(resourceType2));

            ResourceType resourceType3 = ResourceType.None;
            Assert.AreEqual(1, resourceType1.CompareTo(resourceType3));
            Assert.AreEqual(1, resourceType2.CompareTo(resourceType3));
        }

        [Test]
        public void CompareToMinusOneResourceType()
        {
            ResourceType resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            ResourceType resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/");
            Assert.AreEqual(-1, resourceType1.CompareTo(resourceType2));

            ResourceType resourceType3 = ResourceType.None;
            Assert.AreEqual(-1, resourceType3.CompareTo(resourceType1));
            Assert.AreEqual(-1, resourceType3.CompareTo(resourceType2));
        }

        [Test]
        public void EqualsMethodTrueResourceType()
        {
            ResourceType resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test/providers/Microsoft.Web/sites/autoreport");
            ResourceType resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test/providers/Microsoft.Web/sites/autoreport");
            Assert.IsTrue(resourceType1.Equals(resourceType2));

            resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            Assert.IsTrue(resourceType1.Equals(resourceType2));

            ResourceType resourceType3 = ResourceType.None;
            ResourceType resourceType4 = ResourceType.None;
            Assert.IsTrue(resourceType3.Equals(resourceType4));
        }

        [Test]
        public void EqualsMethodFalseResourceType()
        {
            ResourceType resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            ResourceType resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/");
            Assert.IsFalse(resourceType1.Equals(resourceType2));

            ResourceType resourceType3 = ResourceType.None;
            Assert.IsFalse(resourceType3.Equals(resourceType1));
            Assert.IsFalse(resourceType3.Equals(resourceType2));
        }

        [Test]
        public void CompareToZeroString()
        {
            ResourceType resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test/providers/Microsoft.Web/sites/autoreport");
            ResourceType resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test/providers/Microsoft.Web/sites/autoreport");
            Assert.AreEqual(0, resourceType1.CompareTo(resourceType2.ToString()));

            resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            Assert.AreEqual(0, resourceType1.CompareTo(resourceType2.ToString()));

            ResourceType resourceType3 = ResourceType.None;
            ResourceType resourceType4 = ResourceType.None;
            Assert.AreEqual(0, resourceType3.CompareTo(resourceType4.ToString()));
        }

        [Test]
        public void CompareToOneString()
        {
            ResourceType resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test/providers/Microsoft.Web/sites/autoreport");
            ResourceType resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101");
            Assert.AreEqual(1, resourceType1.CompareTo(resourceType2.ToString()));

            ResourceType resourceType3 = ResourceType.None;
            Assert.AreEqual(1, resourceType1.CompareTo(resourceType3.ToString()));
            Assert.AreEqual(1, resourceType2.CompareTo(resourceType3.ToString()));
        }

        [Test]
        public void CompareToMinusOneString()
        {
            ResourceType resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            ResourceType resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/");
            Assert.AreEqual(-1, resourceType1.CompareTo(resourceType2.ToString()));

            ResourceType resourceType3 = ResourceType.None;
            Assert.AreEqual(-1, resourceType3.CompareTo(resourceType1.ToString()));
            Assert.AreEqual(-1, resourceType3.CompareTo(resourceType2.ToString()));
        }

        [Test]
        public void EqualsMethodTrueString()
        {
            ResourceType resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test/providers/Microsoft.Web/sites/autoreport");
            ResourceType resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test/providers/Microsoft.Web/sites/autoreport");
            Assert.IsTrue(resourceType1.Equals(resourceType2.ToString()));

            resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            Assert.IsTrue(resourceType1.Equals(resourceType2.ToString()));

            ResourceType resourceType3 = ResourceType.None;
            ResourceType resourceType4 = ResourceType.None;
            Assert.IsTrue(resourceType3.Equals(resourceType4.ToString()));
        }

        [Test]
        public void EqualsMethodFalseString()
        {
            ResourceType resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            ResourceType resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/");
            Assert.IsFalse(resourceType1.Equals(resourceType2.ToString()));

            ResourceType resourceType3 = ResourceType.None;
            Assert.IsFalse(resourceType3.Equals(resourceType1.ToString()));
            Assert.IsFalse(resourceType3.Equals(resourceType2.ToString()));
        }

        [Test]
        public void CheckGetHashCode()
        {
            ResourceType resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            string rt1string = resourceType1.ToString();
            ResourceType resourceType2 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/");
            string rt2string = resourceType2.ToString();
            Assert.AreEqual(rt1string.GetHashCode(), resourceType1.GetHashCode());
            Assert.AreEqual(rt2string.GetHashCode(), resourceType2.GetHashCode());

            ResourceType resourceType3 = ResourceType.None;
            string rt3string = resourceType3.ToString();
            Assert.AreEqual(rt3string.GetHashCode(), resourceType3.GetHashCode());
        }

        [Test]
        public void CheckEqualsObjOverride()
        {
            ResourceType resourceType1 = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            Assert.IsFalse(resourceType1.Equals(null));

            object rt1object = new ResourceType("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test");
            Assert.IsTrue(resourceType1.Equals(rt1object));

            object rt1string = "Microsoft/resourceGroups";
            Assert.IsTrue(resourceType1.Equals(rt1string)); //ASK: how to check equals object
        }

        [TestCase("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/")]
        [TestCase("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test")]
        [TestCase("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test/providers/Microsoft.Web/sites/autoreport")]
        public void CheckValidParse(string input)
        {
            ResourceType resourceType = new ResourceType(input);
            Assert.IsNotNull(resourceType);
        }

        [TestCase("")]
        [TestCase("/")]
        [TestCase("/subscriptions")]
        [TestCase("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups")]
        public void CheckInvalidParse(string input)
        {
            ResourceType resourceType = new ResourceType(input);
            Assert.Catch<ArgumentOutOfRangeException>(resourceType.Parse(input));//need to catch exception when creating new ResourceType //ASK: Can't catch exception
        }

        [TestCase("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/")]
        [TestCase("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test")]
        [TestCase("/subscriptions/6b085460-5f21-477e-ba44-1035046e9101/resourceGroups/nbhatia_test/providers/Microsoft.Web/sites/autoreport")]
        public void CheckParent(string input)
        {
            ResourceType resourceType = new ResourceType(input);
            Console.WriteLine(resourceType.Parent);
            Assert.AreEqual(resourceType.Parent, "/");//ASK: Parent always "/"
        }
    }
}
