﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Directum238Bot {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Directum238BotResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Directum238BotResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Directum238Bot.Directum238BotResources", typeof(Directum238BotResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отличное пожелание! Твое сообщение будет доставлено коллеге {0}.
        /// </summary>
        internal static string AfterMessageSaveMessage {
            get {
                return ResourceManager.GetString("AfterMessageSaveMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Привет!
        ///
        ///Вот это быстро пролетело время с нового года. Теперь впереди новые праздники и приятные выходные :)
        ///
        ///А чтобы приятного в нашей жизни стало больше, то давай обмениваться поздравлениями с коллегами к 23 февраля и 8 марта.
        ///
        ///💌 Напиши сообщение, запиши видео-кружок или голосовое. Его сможет получить #directum\_people из любого города. Стираем границы и объединяемся.
        ///
        ///_Помни!!! Твое сообщение может получить, как коллега с соседнего стола, так и твой тимлид, руководитель и топ-менеджер_ 
        ///.
        /// </summary>
        internal static string BotStartMessage {
            get {
                return ResourceManager.GetString("BotStartMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Получить поздравление с 23 февраля.
        /// </summary>
        internal static string GetWish23 {
            get {
                return ResourceManager.GetString("GetWish23", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Получить поздравление с 8 марта.
        /// </summary>
        internal static string GetWish8 {
            get {
                return ResourceManager.GetString("GetWish8", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В главное меню ↩.
        /// </summary>
        internal static string GoStartMenu {
            get {
                return ResourceManager.GetString("GoStartMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пока что поздравлений нет. Зайди позже :).
        /// </summary>
        internal static string NoWishesYet {
            get {
                return ResourceManager.GetString("NoWishesYet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Поздравить с 23 февраля.
        /// </summary>
        internal static string SendWish23 {
            get {
                return ResourceManager.GetString("SendWish23", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Поздравить с 8 марта.
        /// </summary>
        internal static string SendWish8 {
            get {
                return ResourceManager.GetString("SendWish8", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отправить 💌.
        /// </summary>
        internal static string SendWishButton {
            get {
                return ResourceManager.GetString("SendWishButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Остановись, выдохни. Это поздравление ты смог бы сказать человеку лично? Если да, то жми отправить)).
        /// </summary>
        internal static string SendWishConfirmationMessage {
            get {
                return ResourceManager.GetString("SendWishConfirmationMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пришли мне поздравление (текст, голосовое, видеосообщение).
        /// </summary>
        internal static string SendWishToMe {
            get {
                return ResourceManager.GetString("SendWishToMe", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Я такое не понимаю. Могу только текст, голосовые, и видеосообщения.
        /// </summary>
        internal static string UnknownMessageType {
            get {
                return ResourceManager.GetString("UnknownMessageType", resourceCulture);
            }
        }
    }
}
