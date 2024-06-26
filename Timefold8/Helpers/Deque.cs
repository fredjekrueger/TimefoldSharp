﻿using System.Collections;

namespace TimefoldSharp.Core.Helpers
{
    public interface IDeque<T>
    {
        /// <summary> Add a new element to the front of the queue. </summary>
        /// <param name="V"> The item to add to the queue</param>
        void PushFront(T V);
        /// <summary> Add a new element to the back of the queue. </summary>
        /// <param name="V"> The item to add to the queue</param>
        void PushBack(T V);

        void Clear();
        /// <summary> Removes and returns the element at the front of the queue. </summary>
        /// <exception cref="InvalidOperationException">Thrown if Pop is called when the Deque is empty.</exception>
        T PopFront();
        /// <summary> Removes and returns the element at the back of the queue. </summary>
        /// <exception cref="InvalidOperationException">Thrown if Pop is called when the Deque is empty.</exception>
        T PopBack();

        /// <summary> Returns the element at the front of the queue without removing it. 
        ///     If the Deque is empty, the default for the type "T" is returned. </returns>
        T PeekFront { get; }
        /// <summary> Returns the element at the front of the queue without removing it. 
        ///     If the Deque is empty, the default for the type "T" is returned. </returns>
        T PeekBack { get; }

        /// <summary> Gets the number of elements in the queue. </summary>
        int Count { get; }

        /// <summary> True if there are no elements in the queue, false otherwise. </summary>
        bool IsEmpty { get; }
    }

    public class Deque<T> : IDeque<T>, IEnumerable<T>
    {
        int initialAllocation = 0;
        #region Constructors
        public Deque(int InitialAllocation = 16)
        {
            this.initialAllocation = InitialAllocation;
            Reset();

            empty_T = (new T[1])[0];    // Trick to get the default uninitialized
                                        //value for type <T>, without any boxing
                                        //or null-conversion issues.
        }
        #endregion

        public override string ToString()
        {
            return "Dequeue count: " + Count;
        }

        void Reset()
        {
            Count = 0;
            size_ = initialAllocation;
            buffer_ = new T[size_];

            front_ = 0;
            back_ = decrement(front_);
        }

        #region IDeque Properties
        public T PeekFront { get { return buffer_[front_]; } }
        public T PeekBack { get { return buffer_[back_]; } }

        public int Count { get; protected set; }
        public bool IsEmpty => (Count == 0);
        #endregion


        #region IDeque Methods
        public T PopBack()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Cannot Pop from empty Deque!");

            T val = buffer_[back_];         // save the value
            buffer_[back_] = empty_T;       // erase the cell
            back_ = decrement(back_);       // move the array window
            Count--;                        // adjust the count

            return val;
        }

        public T PopFront()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Cannot Pop from empty Deque!");

            T val = buffer_[front_];        // save the value
            buffer_[front_] = empty_T;      // erase the cell
            front_ = increment(front_);     // move the array window
            Count--;                        // adjust the count

            return val;
        }

        public void Clear()
        {
            Reset();
        }

        public void PushBack(T V)
        {
            if (IsFull) ExpandBuffer();
            back_ = increment(back_);
            buffer_[back_] = V;
            Count++;
        }

        public void PushFront(T V)
        {
            if (IsFull) ExpandBuffer();
            front_ = decrement(front_);
            buffer_[front_] = V;
            Count++;
        }
        #endregion


        #region Local Properties and Fields
        protected T[] buffer_;  // holds the array window
        protected int size_;    // allocation size of buffer
        protected int back_;    // back is the tail or LEADING end
        protected int front_;   // front is the head or TRAILING end
        protected T empty_T;    // what should be in an uninitialized cell

        protected int RemainingAllocation => size_ - Count;

        /// <summary> True if there are no empty cells left in the buffer, false otherwise. </summary>
        protected bool IsFull => (RemainingAllocation <= 0);
        //protected bool IsFull => ((front_ == back_ + 1) || (front_ == 0 && back_ == size_ - 1));
        #endregion

        #region Buffer Methods (local)
        /// <summary> returns the next cell's index in the circular buffer. </summary>
        protected int increment(int index) => index == size_ - 1 ? 0 : index + 1;

        /// <summary> returns the preceding cell's index in the circular buffer. </summary>
        protected int decrement(int index) => index == 0 ? size_ - 1 : index - 1;

        /// <summary>
        ///     Double the size of the current buffer, without losing any contents
        /// </summary>
        /// <remarks>
        ///     Makes a new buffer twice a large as the current one.  Then copies
        ///   the contents of the old buffer to the new one, then makes the new
        ///   buffer the current ("buffer_").
        /// </remarks>
        void ExpandBuffer()
        {
            // allocate the new buffer
            int newSize = size_ * 2;
            T[] newBuf = new T[newSize];

            // copy the data over
            if (!IsEmpty)
            {
                //  if the pointers are in-order, then all contents can
                // be copied to the same indexes in the new buffer
                if (front_ <= back_)
                {
                    for (int i = front_; i <= back_; i++)
                    {
                        newBuf[i] = buffer_[i];
                    }
                }

                //  The contents are wrapped-around the circular buffer so
                // reposition the back (wrapped-around) contents to after
                // the front contents.
                else
                {
                    //  The front cells can be copied to the same indexes in the
                    // new buffer.
                    for (int i = front_; i < size_; i++)
                    {
                        newBuf[i] = buffer_[i];
                    }
                    //  The back cells are wrapped-around, so reposition them to
                    // follow immediately after the front cells.
                    for (int i = 0; i <= back_; i++)
                    {
                        newBuf[i + size_] = buffer_[i];
                    }
                }

            }

            // reposition the back_ pointer so it's not wrapped-around
            if (back_ < front_) back_ += size_;

            // save the new buffer 
            buffer_ = newBuf;
            size_ = newSize;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return buffer_.OfType<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return buffer_.GetEnumerator();
        }
        #endregion
    }
}