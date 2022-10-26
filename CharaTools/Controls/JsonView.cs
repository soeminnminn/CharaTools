using System;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json.Linq;
using CharaTools.Models;

namespace CharaTools.Controls
{
    [TemplatePart(Name = PART_Items, Type = typeof(ItemsControl))]
    internal class JsonView : Control
    {
        #region Variables
        public const string PART_Items = "PART_Items";

        private ItemsControl itemsControl = null;
        #endregion

        #region Dependency Properties
        /// <summary>
        /// Identifies the <see cref="Source"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source), typeof(string), typeof(JsonView),
            new UIPropertyMetadata(null, (s, e) => ((JsonView)s).OnSourceChanged(e)));
        #endregion

        #region Constructors
        static JsonView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JsonView), new FrameworkPropertyMetadata(typeof(JsonView)));
        }

        public JsonView()
        { }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set { SetValue(SourceProperty, value); }
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (Template.FindName(PART_Items, this) is ItemsControl control)
            {
                itemsControl = control;
            }

            ApplySource(Source);
        }

        private void ApplySource(string json)
        {
            if (itemsControl != null && !string.IsNullOrEmpty(json))
            {
                var jObject = JObject.Parse(json);
                var root = new JsonItem("(root)", jObject, 0);
                itemsControl.ItemsSource = root.GetChildren();
            }
        }

        /// <summary>
        /// Handles changes in the HierarchySource.
        /// </summary>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string json)
            {
                ApplySource(json);
            }
        }
        #endregion
    }
}
