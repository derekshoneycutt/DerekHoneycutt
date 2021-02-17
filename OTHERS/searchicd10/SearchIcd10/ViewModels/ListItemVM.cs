using SearchIcd10.Data;
using SearchIcd10.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace SearchIcd10.ViewModels
{
    /// <summary>
    /// View Model Class for List Items
    /// </summary>
    public class ListItemVM : NotifyingModel
    {
        private static readonly string m_CommentStart = "  - ";
        /// <summary>
        /// Gets the Starting Comment for an item, representing an essentially blank comment
        /// </summary>
        public static string CommentStart { get { return m_CommentStart; } }

        public static readonly PropertyChangedEventArgs TitleChangedArgs = new PropertyChangedEventArgs("Title");
        public static readonly PropertyChangedEventArgs ParentTitleChangedArgs = new PropertyChangedEventArgs("ParentTitle");
        public static readonly PropertyChangedEventArgs ModifyingCommentChangedArgs = new PropertyChangedEventArgs("ModifyingComment");
        public static readonly PropertyChangedEventArgs CommentChangedArgs = new PropertyChangedEventArgs("Comment");
        public static readonly PropertyChangedEventArgs HasOwnCommentChangedArgs = new PropertyChangedEventArgs("HasOwnComment");
        public static readonly PropertyChangedEventArgs IsChosenChangedArgs = new PropertyChangedEventArgs("IsChosen");
        public static readonly PropertyChangedEventArgs ChildrenChangedArgs = new PropertyChangedEventArgs("Children");
        public static readonly PropertyChangedEventArgs SpecificityCompleteChangedArgs = new PropertyChangedEventArgs("SpecificityComplete");

        /// <summary>
        /// The Model that this View Model is Built from
        /// </summary>
        public IcdCodeItem Model { get; private set; }
        
        /// <summary>
        /// The Parent Search View Model object
        /// </summary>
        public SearchVM ParentVM { get; set; }

        /// <summary>
        /// The Parent Item View Model object
        /// </summary>
        public ListItemVM ParentItemVM { get; set; }

        private NumberedTitle m_Title;
        /// <summary>
        /// The Numbered Title of the list item
        /// </summary>
        public NumberedTitle Title
        {
            get { return m_Title; }
            set { SetValue(ref m_Title, value, TitleChangedArgs); }
        }

        private IEnumerable<string> m_ParentTitle;
        /// <summary>
        /// The Title of the Parent item for this item
        /// </summary>
        public IEnumerable<string> ParentTitle
        {
            get { return m_ParentTitle; }
            private set { SetValue(ref m_ParentTitle, value, ParentTitleChangedArgs); }
        }

        private string m_Comment;
        /// <summary>
        /// The comment text associated to this item
        /// </summary>
        public string Comment
        {
            get { return m_Comment; }
            set
            {
                if (SetValue(ref m_Comment, value, CommentChangedArgs))
                {
                    Model.Comment = m_Comment;
                    if (ParentVM != null)
                    {
                        ParentVM.UpdateChecks();
                    }
                }
            }
        }

        private bool m_HasOwnComment;
        /// <summary>
        /// Gets or Sets whether this item has its own comment to display
        /// </summary>
        public bool HasOwnComment
        {
            get { return m_HasOwnComment; }
            set { SetValue(ref m_HasOwnComment, value, HasOwnCommentChangedArgs); }
        }

        private bool m_SpecificityComplete;
        public bool SpecificityComplete
        {
            get { return m_SpecificityComplete; }
            set { SetValue(ref m_SpecificityComplete, value, SpecificityCompleteChangedArgs); }
        }

        /// <summary>
        /// Checks if the parent item has a specific code already enabled
        /// </summary>
        /// <param name="item">Code item to check parent for</param>
        /// <returns>True if parent does have the enabled code</returns>
        private bool ParentHasEnabledCode(IcdCodeItem item)
        {
            if (ParentItemVM != null)
            {
                if (ParentItemVM.m_Children != null)
                {
                    if (ParentItemVM.m_Children.Any(vm => vm.IsChosen && (String.Equals(vm.Model.Code.Code, item.Code.Code))))
                    {
                        return true;
                    }
                }
                return ParentItemVM.ParentHasEnabledCode(item);
            }
            return false;
        }

        /// <summary>
        /// Update Comments and Children according to whether the item is chosen or not
        /// </summary>
        private void UpdateChosen()
        {
            if (IsChosen)
            {
                if (m_Children == null)
                {
                    Children = CodesToItems.GetChildItems(
                        Model.Children
                                .Where(ci => !ParentHasEnabledCode(ci)),
                        ParentVM, this);
                    if (m_Children.Count == 0)
                    {
                        SpecificityComplete = true;
                    }
                    else
                    {
                        if (!String.Equals(m_Children[0].Title.Title, CodesToItems.SpecificityString))
                        {
                            SpecificityComplete = true;
                        }
                    }
                    m_TrueChildren.Clear();
                    m_TrueChildren.AddRange(Children);
                }
                if ((ParentItemVM != null) && (String.Equals(Model.Code.ChildType, Data.IcdCodeStrings.ChildType_Direct)))
                {
                    Comment = ParentItemVM.Comment;
                    ParentItemVM.Comment = ListItemVM.CommentStart;
                    ParentItemVM.HasOwnComment = false;
                }
            }
            else
            {
                SpecificityComplete = false;
                foreach (var child in m_TrueChildren)
                {
                    child.IsChosen = false;
                }
                m_TrueChildren.Clear();
                if (HasOwnComment)
                {
                    if ((ParentItemVM != null) && (String.Equals(Model.Code.ChildType, Data.IcdCodeStrings.ChildType_Direct)))
                    {
                        ParentItemVM.Comment = Comment;
                        ParentItemVM.HasOwnComment = true;
                        Comment = CommentStart;
                    }
                }
                Children = null;
            }


            if (ParentVM != null)
            {
                if (ParentItemVM == null)
                {
                    if (IsChosen)
                    {
                        ParentVM.ClearToItem(this);
                    }
                    else
                    {
                        ParentVM.ClearToFullList();
                    }
                }
                else
                {
                    if (String.Equals(Model.Code.ChildType, IcdCodeStrings.ChildType_Direct))
                    {
                        if (IsChosen)
                        {
                            ParentItemVM.ClearChildrenToSelected();
                            ParentItemVM.ReplaceSelectedChild(this);
                        }
                        else
                        {
                            ClearForParent();
                        }
                    }
                    ParentVM.UpdateChecks();
                }
            }
        }

        private bool m_IsChosen;
        /// <summary>
        /// Whether this item has been chosen and thus enabled or not
        /// </summary>
        public bool IsChosen
        {
            get { return m_IsChosen; }
            set
            {
                if (SetValue(ref m_IsChosen, value, IsChosenChangedArgs))
                {
                    Model.Enabled = m_IsChosen;
                    UpdateChosen();
                }
            }
        }

        private List<ListItemVM> m_TrueChildren;
        private List<ListItemVM> m_Children;
        /// <summary>
        /// Gets children items for this item
        /// </summary>
        public IEnumerable<ListItemVM> Children
        {
            get
            {
                if (m_Children == null)
                {
                    return Enumerable.Empty<ListItemVM>();
                }
                return m_Children;
            }
            private set
            {
                if (value != null)
                {
                    m_Children = value.ToList();
                }
                else
                {
                    m_Children = null;
                }
                RaisePropertyChanged(ChildrenChangedArgs);
            }
        }

        /// <summary>
        /// Command to be called when a comment is to be added to the item
        /// </summary>
        public ICommand CommentCommand { get; private set; }
        /// <summary>
        /// Command to be called to toggle the IsChosen property
        /// </summary>
        public ICommand ToggleChosenCmnd { get; private set; }

        /// <summary>
        /// Initiate a new ListItemVM object
        /// </summary>
        /// <param name="number">The number to be associated to the item</param>
        /// <param name="item">The IcdCodeItem object that will serve as the model</param>
        /// <param name="parentTitle">The object's parent's title</param>
        public ListItemVM(int number, IcdCodeItem item, IEnumerable<string> parentTitle, bool showCode = false)
        {
            SpecificityComplete = false;
            HasOwnComment = true;
            m_TrueChildren = new List<ListItemVM>();
            Model = item;
            Comment = CommentStart;
            Title = new NumberedTitle(number, 
                        (showCode) ? 
                            (item.Code.CodeType.Equals(IcdCodeStrings.CodeType_Divider)) ?
                                item.Code.Title
                                : String.Format("{0}: {1}", item.Code.Code, item.Code.Title) 
                            : item.Code.Title);
            ParentTitle = parentTitle;

            m_Comment = item.Comment;
            m_IsChosen = item.Enabled;

            CommentCommand = new ActionCommand(ModifyComment);

            ToggleChosenCmnd = new ActionCommand(() => IsChosen = !IsChosen);
        }

        /// <summary>
        /// Modify the existing comment or add a new comment to the item
        /// </summary>
        public void ModifyComment()
        {
            if (String.IsNullOrWhiteSpace(Comment))
            {
                Comment = CommentStart;
            }
            else
            {
                Comment = null;
            }
        }

        /// <summary>
        /// Renumber the item and any children items
        /// </summary>
        /// <param name="startAt">The number to start renumbering at</param>
        /// <returns>The last number used for renumbering</returns>
        public int Renumber(int startAt)
        {
            int ret = startAt;
            Title = new NumberedTitle(ret, Title.Title);
            if (m_Children != null)
            {
                foreach (var child in Children)
                {
                    if (!String.Equals(child.Model.Code.CodeType, IcdCodeStrings.CodeType_Divider))
                    {
                        ++ret;
                        ret = child.Renumber(ret);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Look for a shown child and replace it with a new one
        /// <para>This comes as we have set it up for items to be replaced in the hierarchy</para>
        /// </summary>
        /// <param name="shown">Shown item to replace</param>
        /// <param name="newItem">New item to replace the shown with</param>
        private void ReplaceShownChild(ListItemVM shown, ListItemVM newItem)
        {
            if (ParentItemVM == null)
            {
                if (ParentVM != null)
                {
                    ParentVM.ReplaceSelectedItem(newItem);
                }
            }
            else
            {
                if (String.Equals(Model.Code.ChildType, IcdCodeStrings.ChildType_Direct))
                {
                    ParentItemVM.ReplaceShownChild(shown, newItem);
                }
                else
                {
                    var newParentChildren = ParentItemVM.Children.ToList();
                    for (int childOn = 0; childOn < newParentChildren.Count; ++childOn)
                    {
                        if (Object.ReferenceEquals(newParentChildren[childOn], shown) ||
                            Object.ReferenceEquals(newParentChildren[childOn], this))
                        {
                            newParentChildren[childOn] = newItem;
                        }
                    }
                    ParentItemVM.Children = newParentChildren;
                }
            }
        }

        /// <summary>
        /// Replace a single, selected item
        /// <para>Attempts to propagate to the ListVM of ParentVM or calls ReplaceShownChild when appropriate</para>
        /// </summary>
        /// <param name="newItem">New item to replace the single selected item with</param>
        public void ReplaceSelectedChild(ListItemVM newItem)
        {
            if (newItem == null)
            {
                return;
            }

            if (ParentItemVM == null)
            {
                if (ParentVM != null)
                {
                    ParentVM.ReplaceSelectedItem(newItem);
                }
            }
            else
            {
                ReplaceShownChild(this, newItem);
            }
        }

        /// <summary>
        /// Attempt to clear the ParentVM ListVM when appropriate.
        /// <para>Will figure if needs to load </para>
        /// </summary>
        public void ClearForParent()
        {
            if (String.Equals(Model.Code.ChildType, IcdCodeStrings.ChildType_Direct))
            {
                if ((ParentVM != null) && (ParentItemVM != null))
                {
                    ParentItemVM.ClearToAllChildren();
                    if (Object.ReferenceEquals(ParentVM.Items.Items[0], this))
                    {
                        ParentVM.ReplaceSelectedItem(ParentItemVM, false);
                    }
                    else
                    {
                        ReplaceShownChild(this, ParentItemVM);
                    }
                }
                else if (ParentVM != null)
                {
                    ParentVM.ClearToFullList();
                }
            }
        }

        /// <summary>
        /// Clear out children so only the selected items are included in the children
        /// </summary>
        public void ClearChildrenToSelected()
        {
            if (m_Children != null)
            {
                var newList = new List<ListItemVM>();
                foreach (var child in m_Children)
                {
                    if (String.Equals(child.Model.Code.CodeType, IcdCodeStrings.CodeType_Divider))
                    {
                        newList.Add(child);
                    }
                    else
                    {
                        if (child.IsChosen)
                        {
                            newList.Add(child);
                        }
                    }
                }

                Children = newList;
            }
        }

        /// <summary>
        /// Re-setup children so that all of the children are displayed
        /// </summary>
        public void ClearToAllChildren()
        {
            if (m_TrueChildren.Count > 0)
            {
                Children = m_TrueChildren;
            }
        }
    }
}
