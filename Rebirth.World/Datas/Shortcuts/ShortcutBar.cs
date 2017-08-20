using Rebirth.Common.IO;
using Rebirth.Common.Pool;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Messages;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Datas.Characters;
using Rebirth.World.Datas.Items;
using Rebirth.World.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebirth.World.Datas.Shortcuts
{
    public class ShortcutBar
    {
        #region Vars
        public const int MaxSlot = 40;
        private readonly object m_locker = new object();
        private Dictionary<sbyte, SpellShortcut> m_spellShortcuts = new Dictionary<sbyte, SpellShortcut>();
        private Dictionary<sbyte, ItemShortcut> m_itemShortcuts = new Dictionary<sbyte, ItemShortcut>();
        private Dictionary<sbyte, PresetShortCut> m_presetShortcuts = new Dictionary<sbyte, PresetShortCut>();
        #endregion

        #region Properties
        public Character Owner { get; private set; }
        #endregion

        #region Constructor / Datas
        public ShortcutBar(Character owner)
        {
            this.Owner = owner;
        }

        public ShortcutBar(Character owner, byte[] datas)
        {
            Owner = owner;
            var reader = new BigEndianReader(datas);
            var count = reader.ReadShort();
            for (int i = 0; i < count; i++)
            {
                m_spellShortcuts.Add(reader.ReadSByte(), new SpellShortcut(reader));
            }
            count = reader.ReadShort();
            for (int i = 0; i < count; i++)
            {
                m_itemShortcuts.Add(reader.ReadSByte(), new ItemShortcut(reader));
            }
            count = reader.ReadShort();
            for (int i = 0; i < count; i++)
            {
                m_presetShortcuts.Add(reader.ReadSByte(), new PresetShortCut(reader));
            }
        }

        public byte[] GetDatas()
        {
            var writer = new BigEndianWriter();
            writer.WriteShort((short)m_spellShortcuts.Count);
            foreach (var item in m_spellShortcuts)
            {
                writer.WriteSByte(item.Key);
                writer.WriteBytes(item.Value.GetDatas());
            }
            writer.WriteShort((short)m_itemShortcuts.Count);
            foreach (var item in m_itemShortcuts)
            {
                writer.WriteSByte(item.Key);
                writer.WriteBytes(item.Value.GetDatas());
            }
            writer.WriteShort((short)m_presetShortcuts.Count);
            foreach (var item in m_presetShortcuts)
            {
                writer.WriteSByte(item.Key);
                writer.WriteBytes(item.Value.GetDatas());
            }
            return writer.Data;
        }
        #endregion

        #region Methods
        public PresetShortCut GetPresetShortcutBydId(int id)
        {
            return this.m_presetShortcuts.Values.FirstOrDefault(e => e.PresetId == id);
        }
        public PresetShortCut GetPresetShortcut(sbyte slot)
        {
            PresetShortCut presetShortcut;
            return this.m_presetShortcuts.TryGetValue(slot, out presetShortcut) ? presetShortcut : null;
        }
        public void AddShortcut(ShortcutBarEnum barType, Shortcut shortcut)
        {
            if (shortcut is ShortcutSpell && barType == ShortcutBarEnum.SPELL_SHORTCUT_BAR)
            {
                this.AddSpellShortcut(shortcut.slot, (shortcut as ShortcutSpell).spellId);
            }
            else
            {
                if (shortcut is ShortcutObjectItem && barType == ShortcutBarEnum.GENERAL_SHORTCUT_BAR)
                {
                    this.AddItemShortcut(shortcut.slot, this.Owner.Inventory.Items.FirstOrDefault(x => x.Guid == (shortcut as ShortcutObjectItem).itemUID));
                }
                //else if (shortcut is ShortcutObjectPreset)
                //{
                //    var preset = this.Owner.Preset.GetPresetId(((ShortcutObjectPreset)shortcut).presetId);
                //    if (preset == null)
                //        ShortcutFrame.SendShortcutBarAddErrorMessage(this.Owner.Client);
                //    else
                //        this.AddPresetShortcut(shortcut.slot, preset);
                //}
                else
                {
                    this.Owner.Client.Send(new ShortcutBarAddErrorMessage());
                }
            }
        }
        public void AddSpellShortcut(sbyte slot, uint spellId)
        {

            if (!this.IsSlotFree(slot, ShortcutBarEnum.SPELL_SHORTCUT_BAR))
            {
                this.RemoveShortcut(ShortcutBarEnum.SPELL_SHORTCUT_BAR, slot);
            }
            SpellShortcut spellShortcut = new SpellShortcut(slot, spellId);
            this.m_spellShortcuts.Add(slot, spellShortcut);
            Owner.Client?.Send(new ShortcutBarRefreshMessage((sbyte)ShortcutBarEnum.SPELL_SHORTCUT_BAR, spellShortcut.GetNetworkShortcut()));
        }
        public void AddItemShortcut(sbyte slot, PlayerItem item)
        {
            if (item == null)
                return;
            if (!this.IsSlotFree(slot, ShortcutBarEnum.GENERAL_SHORTCUT_BAR))
            {
                this.RemoveShortcut(ShortcutBarEnum.GENERAL_SHORTCUT_BAR, slot);
            }
            ItemShortcut itemShortcut = new ItemShortcut(slot, item.Template.id, item.Guid);
            this.m_itemShortcuts.Add(slot, itemShortcut);
            Owner.Client?.Send(new ShortcutBarRefreshMessage((sbyte)ShortcutBarEnum.GENERAL_SHORTCUT_BAR, itemShortcut.GetNetworkShortcut()));
        }

        //public void AddPresetShortcut(sbyte slot, ItemPresetRecord item)
        //{
        //    if (!this.IsSlotFree(slot, ShortcutBarEnum.GENERAL_SHORTCUT_BAR))
        //    {
        //        this.RemoveShortcut(ShortcutBarEnum.GENERAL_SHORTCUT_BAR, slot);
        //    }
        //    PresetShortCut presetShortcut = new PresetShortCut(this.Owner.Record, slot, item.PresetId);
        //    presetShortcut.PresetId = item.PresetId;
        //    presetShortcut.IsNew = true;
        //    this.m_presetShortcuts.Add(slot, presetShortcut);
        //    ShortcutFrame.SendShortcutBarRefreshMessage(this.Owner.Client, ShortcutBarEnum.GENERAL_SHORTCUT_BAR, presetShortcut);
        //}

        public void SwapShortcuts(ShortcutBarEnum barType, sbyte slot, sbyte newSlot)
        {
            if (!this.IsSlotFree(slot, barType))
            {
                ShortcutRecord shortcut = this.GetShortcut(barType, slot);
                ShortcutRecord shortcut2 = this.GetShortcut(barType, newSlot);
                this.RemoveInternal(shortcut);
                this.RemoveInternal(shortcut2);
                if (shortcut2 != null)
                {
                    shortcut2.Slot = slot;
                    this.AddInternal(shortcut2);
                    Owner.Client?.Send(new ShortcutBarRefreshMessage((sbyte)barType, shortcut2.GetNetworkShortcut()));
                }
                else
                    Owner.Client?.Send(new ShortcutBarRemovedMessage((sbyte)barType, slot));
                shortcut.Slot = newSlot;
                this.AddInternal(shortcut);
                Owner.Client?.Send(new ShortcutBarRefreshMessage((sbyte)barType, shortcut.GetNetworkShortcut()));
            }
        }
        public void RemoveShortcut(ShortcutBarEnum barType, sbyte slot)
        {
            ShortcutRecord shortcut = this.GetShortcut(barType, slot);
            if (shortcut != null)
            {
                if (barType == ShortcutBarEnum.SPELL_SHORTCUT_BAR)
                {
                    this.m_spellShortcuts.Remove(slot);
                }
                else
                {
                    if (barType == ShortcutBarEnum.GENERAL_SHORTCUT_BAR)
                    {
                        if (shortcut is ItemShortcut)
                        {
                            this.m_itemShortcuts.Remove(shortcut.Slot);
                        }
                        else if (shortcut is PresetShortCut)
                        {
                            this.m_presetShortcuts.Remove(shortcut.Slot);
                        }
                    }
                }
                Owner.Client?.Send(new ShortcutBarRemovedMessage((sbyte)barType, slot));
            }
        }
        public void RemoveAll(ShortcutBarEnum barType)
        {
            List<ShortcutRecord> spells = GetShortcuts(barType).ToList();
            foreach (var shortcut in spells)
            {
                if (shortcut != null)
                {
                    if (barType == ShortcutBarEnum.SPELL_SHORTCUT_BAR)
                    {
                        if (shortcut is ItemShortcut)
                        {
                            this.m_itemShortcuts.Remove(shortcut.Slot);
                        }
                        else if (shortcut is PresetShortCut)
                        {
                            this.m_presetShortcuts.Remove(shortcut.Slot);
                        }
                    }
                    Owner.Client?.Send(new ShortcutBarRemovedMessage((sbyte)barType, shortcut.Slot));
                }
            }
        }
        private void AddInternal(ShortcutRecord shortcut)
        {
            if (shortcut is SpellShortcut)
            {
                this.m_spellShortcuts.Add(shortcut.Slot, (SpellShortcut)shortcut);
            }
            else
            {
                if (shortcut is ItemShortcut)
                {
                    this.m_itemShortcuts.Add(shortcut.Slot, (ItemShortcut)shortcut);
                }
            }
        }
        private bool RemoveInternal(ShortcutRecord shortcut)
        {
            bool result;
            if (shortcut is SpellShortcut)
            {
                result = this.m_spellShortcuts.Remove(shortcut.Slot);
            }
            else if (shortcut is PresetShortCut)
            {
                result = this.m_presetShortcuts.Remove(shortcut.Slot);
            }
            else
            {
                result = (shortcut is ItemShortcut && this.m_itemShortcuts.Remove(shortcut.Slot));
            }
            return result;
        }
        public int GetNextFreeSlot(ShortcutBarEnum barType)
        {
            int result;
            for (sbyte i = 0; i < 40; i++)
            {
                if (this.IsSlotFree(i, barType))
                {
                    result = i;
                    return result;
                }
            }
            result = 40;
            return result;
        }
        public bool IsSlotFree(sbyte slot, ShortcutBarEnum barType)
        {
            bool result = true;
            if (barType == ShortcutBarEnum.SPELL_SHORTCUT_BAR)
            {
                result = !this.m_spellShortcuts.ContainsKey(slot);
            }
            else
            {
                if (barType != ShortcutBarEnum.GENERAL_SHORTCUT_BAR)
                    result = false;

                if (this.m_itemShortcuts.ContainsKey(slot))
                    result = false;

                if (this.m_presetShortcuts.ContainsKey(slot))
                    result = false;
            }
            return result;
        }
        public ShortcutRecord GetShortcut(PlayerItem item)
        {
            return m_itemShortcuts.FirstOrDefault(x => x.Value.ItemGuid == item.Guid).Value;
        }
        public ShortcutRecord GetShortcut(ShortcutBarEnum barType, sbyte slot)
        {
            ShortcutRecord result;
            switch (barType)
            {
                case ShortcutBarEnum.GENERAL_SHORTCUT_BAR:
                    {
                        result = this.GetItemShortcut(slot);
                        if (result == null)
                            result = GetPresetShortcut(slot);
                        break;
                    }
                case ShortcutBarEnum.SPELL_SHORTCUT_BAR:
                    result = this.GetSpellShortcut(slot);
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }
        public ShortcutRecord[] GetShortcuts(ShortcutBarEnum barType)
        {
            List<ShortcutRecord> result = new List<ShortcutRecord>();
            switch (barType)
            {
                case ShortcutBarEnum.GENERAL_SHORTCUT_BAR:
                    result.AddRange(m_itemShortcuts.Values);
                    result.AddRange(m_presetShortcuts.Values);
                    break;
                case ShortcutBarEnum.SPELL_SHORTCUT_BAR:
                    result.AddRange(m_spellShortcuts.Values);
                    break;
                default:
                    break;
            }
            return result.ToArray();
        }
        public SpellShortcut GetSpellShortcut(sbyte slot)
        {
            SpellShortcut spellShortcut;
            return this.m_spellShortcuts.TryGetValue(slot, out spellShortcut) ? spellShortcut : null;
        }
        public ItemShortcut GetItemShortcut(sbyte slot)
        {
            ItemShortcut itemShortcut;
            return this.m_itemShortcuts.TryGetValue(slot, out itemShortcut) ? itemShortcut : null;
        }
        #endregion
    }
}