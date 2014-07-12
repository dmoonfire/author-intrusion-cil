﻿// <copyright file="MemoryBufferTests.cs" company="Moonfire Games">
//     Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// MIT Licensed (http://opensource.org/licenses/MIT)
namespace MfGames.TextTokens.Tests
{
    using MfGames.TextTokens.Controllers;
    using MfGames.TextTokens.Texts;

    using NUnit.Framework;

    /// <summary>
    /// </summary>
    [TestFixture]
    public class MemoryBufferTests
    {
        #region Properties

        /// <summary>
        /// Contains an in-memory buffer model.
        /// </summary>
        protected TestBuffer Buffer { get; private set; }

        /// <summary>
        /// Contains a UI controller for the buffer.
        /// </summary>
        protected UserBufferController Controller { get; private set; }

        /// <summary>
        /// Contains a listener which reflects the user-visible state of
        /// the buffer.
        /// </summary>
        protected TestBufferState State { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Generic setup for all memory buffer tests.
        /// </summary>
        protected virtual void Setup()
        {
            KeyGenerator.Instance = new KeyGenerator();
            this.Buffer = new TestBuffer();
            this.Controller = new UserBufferController(this.Buffer);
            this.State = new TestBufferState(this.Buffer);
        }

        #endregion

        /// <summary>
        /// </summary>
        public class InitialState : MemoryBufferTests
        {
            #region Public Methods and Operators

            /// <summary>
            /// </summary>
            [Test]
            public void VerifyInitialState()
            {
                this.Setup();
                Assert.AreEqual(
                    0, this.State.Lines.Count, "Number of lines was unexpected.");
            }

            #endregion
        }

        /// <summary>
        /// </summary>
        [TestFixture]
        public class InsertFiveLines : MemoryBufferTests
        {
            #region Public Methods and Operators

            /// <summary>
            /// </summary>
            [Test]
            public void FifthLineTextIsCorrect()
            {
                this.Setup();
                Assert.AreEqual(
                    "twenty twenty-one twenty-two twenty-three twenty-four", 
                    this.State.Lines[4].Tokens.GetVisibleText());
            }

            /// <summary>
            /// </summary>
            [Test]
            public void FirstLineHasNineTokens()
            {
                this.Setup();
                Assert.AreEqual(9, this.State.Lines[0].Tokens.Count);
            }

            /// <summary>
            /// </summary>
            [Test]
            public void FirstLineTextIsCorrect()
            {
                this.Setup();
                Assert.AreEqual(
                    "zero one two three four", 
                    this.State.Lines[0].Tokens.GetVisibleText());
            }

            /// <summary>
            /// </summary>
            [Test]
            public void ForthLineTextIsCorrect()
            {
                this.Setup();
                Assert.AreEqual(
                    "fifteen sixteen seventeen eighteen nineteen", 
                    this.State.Lines[3].Tokens.GetVisibleText());
            }

            /// <summary>
            /// </summary>
            [Test]
            public void HasFiveLines()
            {
                this.Setup();
                Assert.AreEqual(5, this.State.Lines.Count);
            }

            /// <summary>
            /// </summary>
            [Test]
            public void SecondLineTextIsCorrect()
            {
                this.Setup();
                Assert.AreEqual(
                    "five six seven eight nine", 
                    this.State.Lines[1].Tokens.GetVisibleText());
            }

            /// <summary>
            /// </summary>
            [Test]
            public void ThirdLineTextIsCorrect()
            {
                this.Setup();
                Assert.AreEqual(
                    "ten eleven twelve thirteen fourteen", 
                    this.State.Lines[2].Tokens.GetVisibleText());
            }

            #endregion

            #region Methods

            /// <summary>
            /// </summary>
            protected override void Setup()
            {
                base.Setup();
                this.Buffer.PopulateRowColumn(5, 5);
            }

            #endregion
        }

        /// <summary>
        /// </summary>
        [TestFixture]
        public class InsertTextIntoSingleLineMiddleToken : MemoryBufferTests
        {
            #region Public Methods and Operators

            /// <summary>
            /// </summary>
            [Test]
            public void FirstLineHasCorrectTokenCount()
            {
                this.Setup();
                Assert.AreEqual(5, this.State.Lines[0].Tokens.Count);
            }

            /// <summary>
            /// </summary>
            [Test]
            public virtual void FirstLineTextIsCorrect()
            {
                this.Setup();
                Assert.AreEqual(
                    "zero onBe two", this.State.Lines[0].Tokens.GetVisibleText());
            }

            /// <summary>
            /// Verifies that there is only a single line in the buffer.
            /// </summary>
            [Test]
            public void HasOneLine()
            {
                this.Setup();
                Assert.AreEqual(1, this.State.Lines.Count);
            }

            #endregion

            #region Methods

            /// <summary>
            /// </summary>
            protected override void Setup()
            {
                base.Setup();
                this.Buffer.PopulateRowColumn(1, 3);
                var textLocation = new TextLocation(0, 2, 2);
                this.Controller.InsertText(textLocation, "B");
            }

            #endregion
        }

        /// <summary>
        /// </summary>
        [TestFixture]
        public class InsertTextIntoSingleLineSelection : MemoryBufferTests
        {
            #region Public Methods and Operators

            /// <summary>
            /// </summary>
            [Test]
            public virtual void FirstLineHasCorrectTokenCount()
            {
                this.Setup();
                Assert.AreEqual(1, this.State.Lines[0].Tokens.Count);
            }

            /// <summary>
            /// </summary>
            [Test]
            public virtual void FirstLineTextIsCorrect()
            {
                this.Setup();
                Assert.AreEqual(
                    "zeBo", this.State.Lines[0].Tokens.GetVisibleText());
            }

            /// <summary>
            /// Verifies that there is only a single line in the buffer.
            /// </summary>
            [Test]
            public void HasOneLine()
            {
                this.Setup();
                Assert.AreEqual(1, this.State.Lines.Count);
            }

            #endregion

            #region Methods

            /// <summary>
            /// </summary>
            protected override void Setup()
            {
                base.Setup();
                this.Buffer.PopulateRowColumn(1, 3);
                var anchor = new TextLocation(0, 0, 2);
                var cursor = new TextLocation(0, 4, 2);
                this.Controller.SetCursor(anchor);
                this.Controller.Select(cursor);
                this.Controller.InsertText("B");
            }

            #endregion
        }

        /// <summary>
        /// </summary>
        [TestFixture]
        public class InsertTextIntoSingleTokenSelection : MemoryBufferTests
        {
            #region Public Methods and Operators

            /// <summary>
            /// </summary>
            [Test]
            public virtual void FirstLineHasCorrectTokenCount()
            {
                this.Setup();
                Assert.AreEqual(5, this.State.Lines[0].Tokens.Count);
            }

            /// <summary>
            /// </summary>
            [Test]
            public virtual void FirstLineTextIsCorrect()
            {
                this.Setup();
                Assert.AreEqual(
                    "zero oBe two", this.State.Lines[0].Tokens.GetVisibleText());
            }

            /// <summary>
            /// Verifies that there is only a single line in the buffer.
            /// </summary>
            [Test]
            public void HasOneLine()
            {
                this.Setup();
                Assert.AreEqual(1, this.State.Lines.Count);
            }

            #endregion

            #region Methods

            /// <summary>
            /// </summary>
            protected override void Setup()
            {
                base.Setup();
                this.Buffer.PopulateRowColumn(1, 3);
                var anchor = new TextLocation(0, 2, 1);
                var cursor = new TextLocation(0, 2, 2);
                this.Controller.SetCursor(anchor);
                this.Controller.Select(cursor);
                this.Controller.InsertText("B");
            }

            #endregion
        }

        /// <summary>
        /// </summary>
        [TestFixture]
        public class RedoUndoInsertTextIntoSingleLineMiddleToken :
            UndoInsertTextIntoSingleLineMiddleToken
        {
            #region Public Methods and Operators

            /// <summary>
            /// </summary>
            [Test]
            public override void FirstLineTextIsCorrect()
            {
                this.Setup();
                Assert.AreEqual(
                    "zero onBe two", this.State.Lines[0].Tokens.GetVisibleText());
            }

            #endregion

            #region Methods

            /// <summary>
            /// </summary>
            protected override void Setup()
            {
                base.Setup();
                this.Controller.Redo();
            }

            #endregion
        }

        /// <summary>
        /// </summary>
        public class RedoUndoInsertTextIntoSingleLineSelection :
            UndoInsertTextIntoSingleLineSelection
        {
            #region Public Methods and Operators

            /// <summary>
            /// </summary>
            [Test]
            public override void FirstLineHasCorrectTokenCount()
            {
                this.Setup();
                Assert.AreEqual(1, this.State.Lines[0].Tokens.Count);
            }

            /// <summary>
            /// </summary>
            [Test]
            public override void FirstLineTextIsCorrect()
            {
                this.Setup();
                Assert.AreEqual(
                    "zeBo", this.State.Lines[0].Tokens.GetVisibleText());
            }

            #endregion

            #region Methods

            /// <summary>
            /// </summary>
            protected override void Setup()
            {
                base.Setup();
                this.Controller.Redo();
            }

            #endregion
        }

        /// <summary>
        /// </summary>
        [TestFixture]
        public class UndoInsertTextIntoSingleLineMiddleToken :
            InsertTextIntoSingleLineMiddleToken
        {
            #region Public Methods and Operators

            /// <summary>
            /// </summary>
            [Test]
            public override void FirstLineTextIsCorrect()
            {
                this.Setup();
                Assert.AreEqual(
                    "zero one two", this.State.Lines[0].Tokens.GetVisibleText());
            }

            #endregion

            #region Methods

            /// <summary>
            /// </summary>
            protected override void Setup()
            {
                base.Setup();
                this.Controller.Undo();
            }

            #endregion
        }

        /// <summary>
        /// </summary>
        public class UndoInsertTextIntoSingleLineSelection :
            InsertTextIntoSingleLineSelection
        {
            #region Public Methods and Operators

            /// <summary>
            /// </summary>
            [Test]
            public override void FirstLineHasCorrectTokenCount()
            {
                this.Setup();
                Assert.AreEqual(5, this.State.Lines[0].Tokens.Count);
            }

            /// <summary>
            /// </summary>
            [Test]
            public override void FirstLineTextIsCorrect()
            {
                this.Setup();
                Assert.AreEqual(
                    "zero one two", this.State.Lines[0].Tokens.GetVisibleText());
            }

            #endregion

            #region Methods

            /// <summary>
            /// </summary>
            protected override void Setup()
            {
                base.Setup();
                this.Controller.Undo();
            }

            #endregion
        }

        /// <summary>
        /// </summary>
        [TestFixture]
        public class UndoRedoUndoInsertTextIntoSingleLineMiddleToken :
            RedoUndoInsertTextIntoSingleLineMiddleToken
        {
            #region Public Methods and Operators

            /// <summary>
            /// </summary>
            [Test]
            public override void FirstLineTextIsCorrect()
            {
                this.Setup();
                Assert.AreEqual(
                    "zero one two", this.State.Lines[0].Tokens.GetVisibleText());
            }

            #endregion

            #region Methods

            /// <summary>
            /// </summary>
            protected override void Setup()
            {
                base.Setup();
                this.Controller.Undo();
            }

            #endregion
        }

        /// <summary>
        /// </summary>
        public class UndoRedoUndoInsertTextIntoSingleLineSelection :
            RedoUndoInsertTextIntoSingleLineSelection
        {
            #region Public Methods and Operators

            /// <summary>
            /// </summary>
            [Test]
            public override void FirstLineHasCorrectTokenCount()
            {
                this.Setup();
                Assert.AreEqual(5, this.State.Lines[0].Tokens.Count);
            }

            /// <summary>
            /// </summary>
            [Test]
            public override void FirstLineTextIsCorrect()
            {
                this.Setup();
                Assert.AreEqual(
                    "zero one two", this.State.Lines[0].Tokens.GetVisibleText());
            }

            #endregion

            #region Methods

            /// <summary>
            /// </summary>
            protected override void Setup()
            {
                base.Setup();
                this.Controller.Undo();
            }

            #endregion
        }
    }
}