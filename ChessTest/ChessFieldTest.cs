using ChessLibrary;
using System;
using NUnit.Framework;

namespace ChessTest
{
    [TestFixture]
    public class ChessFieldTest
    {
        [Test]
        public void ChessField_InvalidRow_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => new ChessField('0', 'a'));
            Assert.Throws<ArgumentException>(() => new ChessField('9', 'a'));
        }

        [Test]
        public void ChessField_InvalidColumn_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => new ChessField('1', '`'));
            Assert.Throws<ArgumentException>(() => new ChessField('1', 'i'));
        }

        [Test]
        public void ChessField_ValidCoordinates_ShouldNotThrow()
        {
            Assert.DoesNotThrow(() => new ChessField('1', 'a'));
            Assert.DoesNotThrow(() => new ChessField('8', 'h'));
        }
    }
}
