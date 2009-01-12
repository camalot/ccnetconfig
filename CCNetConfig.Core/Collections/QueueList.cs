using System;
using System.Collections.Generic;
using System.Text;
using CCNetConfig.Core.Components;

namespace CCNetConfig.Core.Collections
{
    /// <summary>
    /// Defines a list of queues.
    /// </summary>
    public class QueueList
        : CloneableList<Queue>
    {
        #region Indexers
        /// <summary>
        /// Gets or sets the <see cref="CCNetConfig.Core.Queue"/> with the specified name.
        /// </summary>
        /// <value></value>
        [ReflectorIgnore]
        public Queue this[string name]
        {
            get
            {
                if (this.Contains(name))
                    return this[IndexOf(name)];
                else
                    return null;
            }
            set
            {
                if (this.Contains(name))
                    this[IndexOf(name)] = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }
        #endregion

        #region Public methods
        #region Contains()
        /// <summary>
        /// Determines whether this contains the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// 	<see langword="true"/> if this contains the specified name; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Contains(string name)
        {
            return this.IndexOf(name) >= 0;
        }
        #endregion

        #region IndexOf()
        /// <summary>
        /// Gets the index of the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public int IndexOf(string name)
        {
            for (int i = 0; i < this.Count; i++)
            {
                Queue queue = this[i];
                if (string.Compare(queue.Name, name, true) == 0)
                    return i;
            }
            return -1;
        }
        #endregion
        #endregion
    }
}
