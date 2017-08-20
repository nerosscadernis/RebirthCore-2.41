using Rebirth.Common.Pool;
using Rebirth.Common.Protocol.Enums;
using Rebirth.Common.Protocol.Types;
using Rebirth.World.Game.Characters.Stats;
using Rebirth.World.Game.Fights.Buffs;
using Rebirth.World.Game.Fights.IA;
using Rebirth.World.Game.Fights.Marks;
using Rebirth.World.Game.Fights.Other;
using Rebirth.World.Game.Fights.Result;
using Rebirth.World.Game.Fights.Team;
using Rebirth.World.Game.Spells;
using Rebirth.World.Game.Spells.Cast;
using Rebirth.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Rebirth.Common.Protocol.Data;
using Rebirth.Common.Protocol.Messages;
using Rebirth.World.Network;
using Rebirth.World.Effects.Handlers.Spells;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Datas.Characters;
using Rebirth.Common.Timers;
using Rebirth.World.Datas.Spells;
using Rebirth.World.Managers;
using Rebirth.World.GamePathfinder;

namespace Rebirth.World.Game.Fights.Actors
{
    public class Fighter : ICloneable
    {
        #region Constructor
        public Fighter(FightTeam team, Fight fight, Look look, bool isReady = false)
        {
            Team = team;
            Fight = fight;
            IsReady = isReady;
            m_buffIdProvider = new UniqueIdProvider(0);
            BuffList = new List<Buff>();
            GlyphList = new List<Glyph>();
            TriggerBuffList = new List<TriggerBuff>();
            IsDisconnected = false;
            Summons = new List<SummonMonster>();
            BaseLook = look;
            EffectsDie = new List<SpellEffectHandler>();
        }
        #endregion

        #region Variables
        private UniqueIdProvider m_buffIdProvider;
        private bool isFirtTurn = true;
        public SpellsHistory SpellHistory = new SpellsHistory();
        public List<Buff> BuffList;
        public List<Glyph> GlyphList;
        public List<TriggerBuff> TriggerBuffList;
        #endregion

        #region Properties
        public Fight Fight
        {
            get;
            private set;
        }
        public int Id
        {
            get;
            protected set;
        }
        public bool IsReady
        {
            get;
            private set;
        }
        public FightDisposition Point
        {
            get;
            set;
        }
        public FightTeam Team
        {
            get;
            private set;
        }
        public bool IsDead
        {
            get;
            private set;
        }
        public bool IsAlive
        {
            get
            {
                return Stats.Health.TotalSafe > 0 && !IsDead;
            }
        }
        public StatsFields Stats
        {
            get;
            set;
        }
        public int BonusTimer
        {
            get;
            set;
        }
        public int Level
        {
            get;
            set;
        }
        public List<SummonMonster> Summons
        {
            get;
            set;
        }
        public AbstractResult Result
        {
            get;
            set;
        }
        public bool IsDisconnected
        {
            get;
            set;
        }
        public bool HaveCustomSkin
        {
            get
            {
                return BuffList.Any(x => x is SkinBuff);
            }
        }
        public Look BaseLook
        {
            get;
            set;
        }
        public bool IsInvisible
        {
            get;
            set;
        }
        public int MpUsedMove
        { get; set; }
        public int ApUseActions
        { get; set; }
        public bool SkipTurn
        {
            get;
            set;
        }
        protected bool Control;
        public bool Controller()
        {
            if (BuffList.Any(x => x is ControlBuff))
            {
                Control = true;
                return true;
            }
            return false;
        }
        public int TurnBeforeDeco
        { get { return 15; } }
        public int TurnDisconnect
        { get; set; }
        public bool IsExclu
        { get; set; }
        public bool IsMyController(int id)
        {
            if ((this as SummonMonster).Summoner.Id == id)
                return true;

            return false;
        }
        public Damage LastDamage
        { get; set; }
        public List<SpellEffectHandler> EffectsDie
        {
            get; set;
        }
        public MapPoint PreviousPosition
        { get; set; }
        public MapPoint StartTurnPoint
        { get; set; }
        #endregion

        #region Function Get
        public virtual GameFightFighterInformations GetFighterInformations()
        {
            return null;
        }
        public virtual GameFightFighterInformations GetFighterPreparation()
        {
            return null;
        }
        public virtual GameFightFighterLightInformations GetGameFightFighterLightInformations()
        {
            return null;
        }
        public virtual FightResultListEntry GetResult()
        {
            return new FightResultListEntry();
        }
        public virtual FightTeamMemberInformations GetTeamInformations()
        {
            return new FightTeamMemberInformations(Id);
        }
        public EntityLook GetCustomLook()
        {
            var custom = BuffList.FirstOrDefault(x => x is SkinBuff);
            if (custom != null)
                return (custom as SkinBuff).CustomLook;
            return BaseLook.GetEntityLook();
        }
        public int PopNextBuffId()
        {
            return this.m_buffIdProvider.Pop();
        }
        public SummonMonster GetSummonByCellId(int id)
        {
            return Summons.FirstOrDefault(x => x.Point.Point.CellId == id);
        }
        public List<SummonMonster> GetSummonByTemplate(int id)
        {
            return Summons.FindAll(x => x.Template.Template.id == id);
        }
        #endregion

        #region Public function
        public void SetReady(bool ready, bool isPass = false)
        {
            IsReady = ready;
            if (isPass)
                new TimerCore(AutoReady, 10000);
            if (!Fight.IsStarting)
            {
                Fight.SetReady(this);
                Fight.CheckStart();
            }
        }
        public void AutoReady()
        {
            IsReady = true;
        }
        public virtual void InflictDirectDamage(Damage damage, bool isDeclanched = false)
        {
            if (damage.Source.BuffList.Any(x => x is PoisseBuff))
                damage.EffectGenerationType = Effect.Instances.EffectGenerationType.MinEffects;
            if (!isDeclanched)
                ApplyTriggerBuff(TriggerBuffType.BEFORE_ATTACKED, damage);
            if (HasState(376))
                if (damage.Source.Point.Point.DistanceTo(Point.Point) <= 1)
                    return;
            if (HasState(375))
                if (damage.Source.Point.Point.DistanceTo(Point.Point) > 1)
                    return;
            if (HasState(269))
                return;
            if (damage.Source.HasState(218))
                return;
            if (damage.AfterReduction > 0)
            {
                SacrificeBuff sacrifice = (SacrificeBuff)BuffList.FirstOrDefault(x => x is SacrificeBuff);
                if (sacrifice != null)
                {
                    sacrifice.Apply(damage);
                }
                else
                {
                    if (Stats[PlayerFields.Shield].Total > 0)
                    {
                        int ShieldLoss = TryShield(damage);

                        int permanant = (damage.AfterReduction - ShieldLoss) * Stats[PlayerFields.Erosion].Total / 100;

                        Stats.Health.PermanentDamages += damage.Permanant;
                        Stats.Health.DamageTaken += damage.RealDamage - ShieldLoss;
                        Fight.ShieldPointChange(this, damage.Source, (damage.AfterReduction - ShieldLoss), permanant, ShieldLoss);

                        if (Stats.Health.Total <= 0)
                        {
                            Die(damage.Source);
                        }
                        ApplyTriggerBuff(TriggerBuffType.AFTER_ATTACKED, damage);
                    }
                    else
                    {
                        Stats.Health.PermanentDamages += damage.Permanant;
                        Stats.Health.DamageTaken += damage.RealDamage;
                        Fight.LifePointChange(this, damage);

                        if (Stats.Health.Total <= 0)
                        {
                            Die(damage.Source);
                        }
                        if (!isDeclanched)
                            ApplyTriggerBuff(TriggerBuffType.AFTER_ATTACKED, damage);
                    }
                }
            }
        }
        public virtual int InflictDamage(Damage damage, bool isDeclanched = false)
        {
            if (damage.Source.BuffList.Any(x => x is PoisseBuff))
                damage.EffectGenerationType = Effect.Instances.EffectGenerationType.MinEffects;
            if (!isDeclanched && !damage.IsVenom)
                ApplyTriggerBuff(TriggerBuffType.BEFORE_ATTACKED, damage);
            if (HasState(376))
                if (damage.Source.Point.Point.DistanceTo(Point.Point) <= 1)
                    return 0;
            if (HasState(375))
                if (damage.Source.Point.Point.DistanceTo(Point.Point) > 1)
                    return 0;
            if (HasState(269))
                return 0;
            if (damage.Source.HasState(218))
                return 0;
            LastDamage = damage;
            if (damage.AfterReduction > 0)
            {
                SacrificeBuff sacrifice = (SacrificeBuff)BuffList.FirstOrDefault(x => x is SacrificeBuff);
                if (sacrifice != null)
                {
                    sacrifice.Apply(damage);
                }
                else
                {
                    if (Stats[PlayerFields.Shield].Total > 0)
                    {
                        int ShieldLoss = TryShield(damage);

                        int permanant = (damage.AfterReduction - ShieldLoss) * Stats[PlayerFields.Erosion].Total / 100;

                        Stats.Health.PermanentDamages += permanant;
                        Stats.Health.DamageTaken += damage.RealDamage - ShieldLoss;
                        Fight.ShieldPointChange(this, damage.Source, (damage.AfterReduction - ShieldLoss), permanant, ShieldLoss);

                        if (Stats.Health.Total <= 0)
                        {
                            Die(damage.Source);
                        }
                        ApplyTriggerBuff(TriggerBuffType.AFTER_ATTACKED, damage);
                        return (damage.AfterReduction - ShieldLoss);
                    }
                    else
                    {
                        Stats.Health.PermanentDamages += damage.Permanant;
                        Stats.Health.DamageTaken += damage.RealDamage;
                        Fight.LifePointChange(this, damage);

                        if (Stats.Health.Total <= 0)
                        {
                            Die(damage.Source);
                        }
                        if (!isDeclanched)
                            ApplyTriggerBuff(TriggerBuffType.AFTER_ATTACKED, damage);
                        return damage.AfterReduction;
                    }
                }
            }
            return 0;
        }
        private int TryShield(Damage damage)
        {
            int ShieldLoss = 0;
            foreach (StatBuff buff in BuffList.FindAll(i => i is StatBuff))
            {
                if (buff.Caracteristic == PlayerFields.Shield)
                {
                    if (damage.AfterReduction - buff.Value < 0)
                    {
                        ShieldLoss += damage.AfterReduction;
                        this.Stats.ShieldTest.Context -= damage.AfterReduction;
                        buff.Value -= (short)damage.AfterReduction;
                        return ShieldLoss;
                    }
                    else
                    {
                        ShieldLoss += buff.Value;
                        this.Stats.ShieldTest.Context -= buff.Value;
                        this.RemoveBuff(buff);
                    }
                }
            }
            return ShieldLoss;
        }
        public virtual void InflictPushDamage(Fighter source, SpellTemplate spell, int dist, bool isSecond = false)
        {
            var damage = new Damage(source, this, dist, spell);
            damage.SetPushDamage(isSecond);
            if (damage.AfterReduction > 0)
            {
                Stats.Health.PermanentDamages += damage.Permanant;
                Stats.Health.DamageTaken += damage.RealDamage;

                Fight.LifePointChange(this, damage);

                if (Stats.Health.Total <= damage.AfterReduction)
                {
                    Die(damage.Source);
                }
            }
        }
        public void SlideTo(Fighter source, MapPoint cell, ActionsEnum action)
        {
            UpdateAuraBuff(Point.Point.CellId);
            PreviousPosition = Point.Point.Clone() as MapPoint; ;
            Point.Point = cell;
            Fight.Slide(source, this, cell.CellId, action);
            Fight.MarkManager.UpdateWall();
            Fight.MarkManager.DeclancheWall(this);
        }
        public void TeleportTo(Fighter source, short cellId, ActionsEnum action)
        {
            UpdateAuraBuff(Point.Point.CellId);
            PreviousPosition = Point.Point.Clone() as MapPoint; ;
            Point.Point = new MapPoint(cellId);
            Fight.Teleport(source, this, cellId, action);
            Fight.MarkManager.UpdateWall();
            Fight.MarkManager.DeclancheWall(this);
        }
        public void ExchangePositions(Fighter with)
        {
            UpdateAuraBuff(Point.Point.CellId);
            FightDisposition cell = this.Point;
            PreviousPosition = Point.Point.Clone() as MapPoint; ;
            with.PreviousPosition = with.Point.Point.Clone() as MapPoint; ;
            this.Point = with.Point;
            with.Point = cell;
            Fight.ExchangePosition(this, with, ActionsEnum.ACTION_CHARACTER_EXCHANGE_PLACES);
            Fight.MarkManager.UpdateWall();
            Fight.MarkManager.DeclancheWall(this);
        }
        public void ApplyTriggerBuff(TriggerBuffType type, object token)
        {
            foreach (var buff in BuffList.FindAll(x => x is TriggerBuff && (x as TriggerBuff).TriggerType == type))
            {
                buff.Apply(token);
            }
        }
        public void ApplyTriggerBuff(TriggerBuff buff, object token)
        {
            Fight.StartSequence(SequenceTypeEnum.SEQUENCE_TRIGGERED);
            buff.Apply(token);
            Fight.EndSequence(SequenceTypeEnum.SEQUENCE_TRIGGERED, false);
            Fight.CheckFightEnd();
        }
        public virtual int HealDirect(int healPoints, Fighter from)
        {
            int num;
            if (this.HasState(76))
            {
                Fight.Heal(this, from, 0);
                num = 0;
            }
            else
            {
                if (Stats.Health.Total + healPoints > Stats.Health.TotalMax)
                    healPoints = Stats.Health.TotalMax - Stats.Health.Total;
                Stats.Health.DamageTaken -= healPoints;
                Fight.Heal(this, from, healPoints);
                num = healPoints;
            }
            return num;
        }
        public int Heal(int healPoints, Fighter from, uint dist, double boost, bool withBoost = true)
        {
            healPoints -= (int)Math.Floor((double)healPoints * (dist * 10) / 100);
            if (Stats[PlayerFields.HealMultiplicator].Total > 0)
                healPoints = (int)((double)healPoints * (double)Stats[PlayerFields.HealMultiplicator].Total / 100d);
            if (healPoints < 0)
                healPoints = 0;
            if (withBoost)
                healPoints = Formulas.CalculateHeal(healPoints, from.Stats);
            if (boost > 0)
                healPoints = (int)((double)healPoints * boost);
            return this.HealDirect(healPoints, from);
        }
        public void Reveal()
        {
            var buff = BuffList.FirstOrDefault(x => x is InvisibleBuff);
            if (buff != null)
            {
                RemoveAndDispellBuff(buff, true);
            }
        }
        #endregion

        #region Buffs
        public void FreeBuffId(int id)
        {
            this.m_buffIdProvider.Push(id);
        }
        public bool BuffMaxStackReached(Buff buff)
        {
            //return buff.Spell.CurrentSpellLevel.maxStack > 0 && buff.Spell.CurrentSpellLevel.maxStack <= Enumerable.Count<Buff>((IEnumerable<Buff>)this.BuffList, (Func<Buff, bool>)(entry => entry.GetType() == buff.GetType() && entry.Effect.EffectId == buff.Effect.EffectId && entry.Spell == buff.Spell && entry.CustomActionId == buff.CustomActionId && (buff is StateBuff ? (buff as StateBuff).State.id == (entry as StateBuff).State.id : true)));
            return (buff.Spell.CurrentSpellLevel.maxStack > 0 && buff.Spell.CurrentSpellLevel.maxStack <= Enumerable.Count<Buff>(BuffList, x => x.Effect.Uid == buff.Effect.Uid));
        }
        public virtual bool AddAndApplyBuff(Buff buff, bool freeIdIfFail = true)
        {
            bool flag;
            if (buff is StateBuff && BuffList.Any(x => x is StateBuff && (x as StateBuff).State.id == (buff as StateBuff).State.id))
                return false;
            if (this.BuffMaxStackReached(buff))
            {
                Buff lastBuff = BuffList.FirstOrDefault(entry => entry.Effect.Uid == buff.Effect.Uid);
                RemoveAndDispellBuff(lastBuff, true);
                if (!(buff is TriggerBuff) || ((buff as TriggerBuff).TriggerType & TriggerBuffType.BUFF_ADDED) == TriggerBuffType.BUFF_ADDED)
                    buff.Apply();
                this.AddBuff(buff, true);
                if (freeIdIfFail)
                    this.FreeBuffId(lastBuff.Id);
                flag = false;
            }
            else
            {
                if (!(buff is TriggerBuff) || ((buff as TriggerBuff).TriggerType & TriggerBuffType.BUFF_ADDED) == TriggerBuffType.BUFF_ADDED)
                    buff.Apply();
                this.AddBuff(buff, true);
                flag = true;
            }
            return flag;
        }
        public bool AddBuff(Buff buff, bool freeIdIfFail = true)
        {
            bool flag;
            if (this.BuffMaxStackReached(buff))
            {
                if (freeIdIfFail)
                    this.FreeBuffId(buff.Id);
                flag = false;
            }
            else
            {
                this.BuffList.Add(buff);
                Fight.BuffAdd(buff);
                flag = true;
            }
            return flag;
        }
        public void RemoveAndDispellBuff(Buff buff, bool ForceDispell = false)
        {
            if (buff != null)
            {
                if (buff.Duration == 0 || ForceDispell)
                {
                    if (buff is StatBuff)
                        Fight.BuffRemove(buff);
                    else
                        Fight.BuffDispell(buff);

                    this.RemoveBuff(buff);
                    buff.Dispell();

                    if (Stats.Health.Total <= 0)
                    {
                        Die(this);
                    }
                }
            }
        }
        public void RemoveAndDispellBuff(int spellId, bool ForceDispell = false)
        {
            foreach (var buff in BuffList.FindAll(x => x.Spell.Spell.id == spellId))
            {
                RemoveAndDispellBuff(buff, ForceDispell);
            }
        }
        public void RemoveBuff(Buff buff)
        {
            this.BuffList.Remove(buff);
            this.FreeBuffId(buff.Id);
            if (buff is TriggerBuff && (buff as TriggerBuff).TriggerType == TriggerBuffType.BUFF_ENDED)
            {
                ApplyTriggerBuff((buff as TriggerBuff), null);
            }
        }
        public bool HasState(int id)
        {
            return BuffList.FindAll(x => x is StateBuff && (x as StateBuff).State.id == id).Count > 0;
        }
        public void RemoveAllBuff()
        {
            var buffs = new List<Buff>();
            buffs.AddRange(BuffList);
            foreach (var item in buffs)
            {
                RemoveAndDispellBuff(item, true);
            }
        }
        public void RemoveAllBuff(Fighter fighter)
        {
            List<Buff> buffs = (from x in BuffList
                                where x.Caster == fighter
                                select x).ToList();
            foreach (var item in buffs)
            {
                RemoveAndDispellBuff(item, true);
            }
        }
        public void ReductionBuffDuration(Fighter source, short value = 1)
        {
            List<Buff> buffs = (from x in BuffList
                                where !(x is DelayedBuff)
                                select x).ToList();
            foreach (Buff item in buffs)
            {
                if (item.DecrementDuration(value))
                    RemoveAndDispellBuff(item, true);
            }
            if (buffs.Count > 0)
                Fight.ModifyEffectDuration(source, this, value);
        }
        public FightDispellableEffectExtendedInformations[] GetEffectsExtended()
        {
            return BuffList.Select(x => x.GetFightDispellableEffectExtendedInformations()).ToArray();
        }
        public void RemoveState(int id)
        {
            var buffs = (from x in BuffList
                         where x is StateBuff && (x as StateBuff).State.id == id
                         select x).ToArray();
            foreach (var item in buffs)
            {
                RemoveAndDispellBuff(item, true);
            }
        }
        public void UpdateAuraBuff(short last)
        {
            foreach (var glyph in Fight.MarkManager.GetGlyphs(last))
            {
                RemoveAndDispellBuff((int)glyph.Spell.spellId, true);
            }
        }
        #endregion

        #region Request
        public void MovementRequest(short[] keyMovements)
        {
            PreviousPosition = Point.Point.Clone() as MapPoint;
            var NextCells = new List<CellMap>();
            var result = Path.BuildFromCompressedFightPath(Fight.Map, keyMovements);

            if (Stats.MP.Total < result.MPCost)
                return;


            List<Fighter> Tacklers = Fight.Fighters.FindAll(x => x.Point.Point.IsAdjacentTo(Point.Point) && x.Team.Team != Team.Team && x.IsAlive && !x.IsInvisible && !x.HasState(96));
            if (Tacklers.Count > 0 && !HasState(96) && !IsInvisible)
            {
                var tackedMp = GetTacledMP(Tacklers);
                var tackedAP = GetTackledAP(Tacklers);
                if (tackedMp > 0 || tackedAP > 0)
                {

                    Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                    if (tackedMp > 0 || tackedAP > 0)
                        Fight.Tackle(this, Tacklers.Select(entry => (double)entry.Id).ToArray());

                    if (tackedMp > 0)
                        UseMP((short)tackedMp, true);

                    if (tackedAP > 0)
                        UseAP((short)tackedAP, true);

                    for (int i = 1; i < result.Cells.Count(); i++)
                    {
                        if (Fight.HasTrap(result.Cells[i].Id) || Fight.MarkManager.HasPortail((short)result.Cells[i].Id) || Fight.HasGlyph(result.Cells[i].Id) || Fight.MarkManager.HasWall((short)result.Cells[i].Id))
                        {
                            result.CutPath(i + 1);
                            break;
                        }
                    }

                    Fight.Move(this, result.Cells.Select(entry => entry.Point.CellId).ToArray());

                    UseMP((short)(result.MPCost));
                    MpUsedMove += result.MPCost;
                }
                else
                {
                    Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                    for (int i = 1; i < result.Cells.Count(); i++)
                    {
                        if (Fight.HasTrap(result.Cells[i].Id) || Fight.MarkManager.HasPortail((short)result.Cells[i].Id) || Fight.HasGlyph(result.Cells[i].Id) || Fight.MarkManager.HasWall((short)result.Cells[i].Id))
                        {
                            result.CutPath(i + 1);
                            break;
                        }
                    }

                    Fight.Move(this, result.Cells.Select(entry => entry.Point.CellId).ToArray());
                    UseMP((short)(result.MPCost));
                    MpUsedMove += result.MPCost;
                }

            }
            else
            {
                Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);

                for (int i = 1; i < result.Cells.Count(); i++)
                {
                    if (Fight.HasTrap(result.Cells[i].Id) || Fight.MarkManager.HasPortail((short)result.Cells[i].Id) || Fight.HasGlyph(result.Cells[i].Id) || Fight.MarkManager.HasWall((short)result.Cells[i].Id))
                    {
                        result.CutPath(i + 1);
                        break;
                    }
                }

                Fight.Move(this, result.Cells.Select(entry => entry.Point.CellId).ToArray());
                UseMP((short)(result.MPCost));
                MpUsedMove += result.MPCost;
            }

            CellMap lastCell = result.Cells.LastOrDefault();
            if (lastCell != null)
            {
                UpdateAuraBuff(Point.Point.CellId);
                Point.Point.CellId = (short)result.Cells.Last().Id;
            }

            var portail = Fight.MarkManager.GetEndPortailByAnkama(Point.Point.CellId);
            if (portail != null)
            {
                portail.Item1.Execute(this);
                portail.Item2.Execute(this);
                ApplyTriggerBuff(TriggerBuffType.PORTAL, null);
                TeleportTo(this, (short)portail.Item2.TargetedCell.Id, ActionsEnum.ACTION_CHARACTER_TELEPORT_ON_SAME_MAP);
            }

            Fight.MarkManager.CheckReactive();

            Fight.MarkManager.ExecuteGlyph(this, TriggerBuffType.AURA);
            Fight.DeclancheTrap(result.Cells.Last().Id);
            Fight.MarkManager.DeclancheWall(this, true);

            Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE, false);

            if (NextCells.Count > 0)
            {
                NextCells.Insert(0, result.Cells.Last());
                NextMove(NextCells);
            }

            Fight.VerifChall(ChallengeActionEnum.Move, this);
        }
        public void NextMove(List<CellMap> keyMovements)
        {
            var NextCells = new List<CellMap>();
            var result = new Path(Fight.Map, keyMovements);

            if (Stats.MP.Total < result.MPCost)
                return;


            List<Fighter> Tacklers = Fight.Fighters.FindAll(x => x.Point.Point.IsAdjacentTo(Point.Point) && x.Team.Team != Team.Team && x.IsAlive && !x.IsInvisible);
            if (Tacklers.Count > 0)
            {
                var tackedMp = GetTacledMP(Tacklers);
                var tackedAP = GetTackledAP(Tacklers);
                if (tackedMp > 0 || tackedAP > 0)
                {

                    Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                    if (tackedMp > 0 || tackedAP > 0)
                        Fight.Tackle(this, Tacklers.Select(entry => (double)entry.Id).ToArray());

                    if (tackedMp > 0)
                        UseMP((short)tackedMp, true);

                    if (tackedAP > 0)
                        UseAP((short)tackedAP, true);

                    for (int i = 1; i < result.Cells.Count(); i++)
                    {
                        if (Fight.HasTrap(result.Cells[i].Id) || Fight.MarkManager.HasPortail((short)result.Cells[i].Id) || Fight.HasGlyph(result.Cells[i].Id) || Fight.MarkManager.HasWall((short)result.Cells[i].Id))
                        {
                            result.CutPath(i + 1);
                            break;
                        }
                    }

                    Fight.Move(this, result.Cells.Select(entry => entry.Point.CellId).ToArray());

                    UseMP((short)(result.MPCost));
                    MpUsedMove += result.MPCost;
                }
                else
                {
                    Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                    for (int i = 1; i < result.Cells.Count(); i++)
                    {
                        if (Fight.HasTrap(result.Cells[i].Id) || Fight.MarkManager.HasPortail((short)result.Cells[i].Id) || Fight.HasGlyph(result.Cells[i].Id) || Fight.MarkManager.HasWall((short)result.Cells[i].Id))
                        {
                            result.CutPath(i + 1);
                            break;
                        }
                    }

                    Fight.Move(this, result.Cells.Select(entry => entry.Point.CellId).ToArray());
                    UseMP((short)(result.MPCost));
                    MpUsedMove += result.MPCost;
                }

            }
            else
            {
                Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);

                for (int i = 1; i < result.Cells.Count(); i++)
                {
                    if (Fight.HasTrap(result.Cells[i].Id) || Fight.MarkManager.HasPortail((short)result.Cells[i].Id) || Fight.HasGlyph(result.Cells[i].Id) || Fight.MarkManager.HasWall((short)result.Cells[i].Id))
                    {
                        result.CutPath(i + 1);
                        break;
                    }
                }

                Fight.Move(this, result.Cells.Select(entry => entry.Point.CellId).ToArray());
                UseMP((short)(result.MPCost));
                MpUsedMove += result.MPCost;
            }

            CellMap lastCell = result.Cells.LastOrDefault();
            if (lastCell != null)
            {
                UpdateAuraBuff(Point.Point.CellId);
                Point.Point.CellId = (short)result.Cells.Last().Id;
            }

            var portail = Fight.MarkManager.GetEndPortailByAnkama(Point.Point.CellId);
            if (portail != null)
            {
                portail.Item1.Execute(this);
                portail.Item2.Execute(this);
                ApplyTriggerBuff(TriggerBuffType.PORTAL, null);
                TeleportTo(this, (short)portail.Item2.TargetedCell.Id, ActionsEnum.ACTION_CHARACTER_TELEPORT_ON_SAME_MAP);
            }

            Fight.MarkManager.CheckReactive();

            Fight.MarkManager.ExecuteGlyph(this, TriggerBuffType.AURA);
            Fight.DeclancheTrap(result.Cells.Last().Id);
            Fight.MarkManager.DeclancheWall(this, true);

            Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE, false);

            if (NextCells.Count > 0)
            {
                NextCells.Insert(0, result.Cells.Last());
                NextMove(NextCells);
            }
        }
        public int GetTacledMP(List<Fighter> Tacklers)
        {
            var num1 = 0;
            if (Tacklers.Count <= 0)
            {
                num1 = 0;
            }
            else
            {
                double num2 = 0.0;
                for (int index = 0; index < Tacklers.Count; ++index)
                {
                    Fighter tackler1 = Tacklers[index];
                    if (index == 0)
                        num2 = this.GetTackledPourcent(tackler1);
                    else
                        num2 *= this.GetTackledPourcent(tackler1);
                }
                double num3 = 1.0 - num2;
                if (num3 < 0.0)
                    num3 = 0.0;
                else if (num3 > 1.0)
                    num3 = 1.0;

                num1 = (int)Math.Floor((double)(this.Stats.MP.Total * num3));// pm perdu
            }
            return (int)num1;
        }
        public int GetTackledAP(List<Fighter> Tacklers)
        {
            int num1;

            if (Tacklers.Count <= 0)
            {
                num1 = 0;
            }
            else
            {
                double num2 = 0.0;
                for (int index = 0; index < Tacklers.Count; ++index)
                {
                    Fighter tackler = Tacklers[index];
                    if (index == 0)
                        num2 = this.GetTackledPourcent(tackler);
                    else
                        num2 *= this.GetTackledPourcent(tackler);
                }
                double num3 = 1.0 - num2;
                if (num3 < 0.0)
                    num3 = 0.0;
                else if (num3 > 1.0)
                    num3 = 1.0;
                num1 = (int)Math.Floor((double)this.Stats.AP.Total * num3);
            }
            return num1;
        }
        public void MovementRequest(Path result)
        {
            PreviousPosition = Point.Point.Clone() as MapPoint; ;
            if (Stats.MP.Total < result.MPCost)
                return;

            List<Fighter> Tacklers = Fight.Fighters.FindAll(x => x.Point.Point.IsAdjacentTo(Point.Point) && x.Team.Team != Team.Team && x.IsAlive);
            if (Tacklers.Count > 0)
            {
                var tackedMp = GetTacledMP(Tacklers);
                var tackedAP = GetTackledAP(Tacklers);
                if (tackedMp > 0 || tackedAP > 0)
                {

                    Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                    if (tackedMp > 0 || tackedAP > 0)
                        Fight.Tackle(this, Tacklers.Select(entry => (double)entry.Id).ToArray());

                    if (tackedMp > 0)
                        UseMP((short)tackedMp, true);

                    if (tackedAP > 0)
                        UseAP((short)tackedAP, true);

                    for (int i = 1; i < result.Cells.Count(); i++)
                    {
                        if (Fight.HasTrap(result.Cells[i].Id))
                        {
                            result.CutPath(i + 1);
                            break;
                        }
                    }

                    Fight.Move(this, result.Cells.Select(entry => entry.Point.CellId).ToArray());

                    UseMP((short)(result.MPCost));
                    MpUsedMove += result.MPCost;
                }
                else
                {
                    Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);

                    for (int i = 1; i < result.Cells.Count(); i++)
                    {
                        if (Fight.HasTrap(result.Cells[i].Id))
                        {
                            result.CutPath(i + 1);
                            break;
                        }
                    }

                    Fight.Move(this, result.Cells.Select(entry => entry.Point.CellId).ToArray());
                    UseMP((short)(result.MPCost));
                    MpUsedMove += result.MPCost;
                }

            }
            else
            {
                Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);

                for (int i = 1; i < result.Cells.Count(); i++)
                {
                    if (Fight.HasTrap(result.Cells[i].Id))
                    {
                        result.CutPath(i + 1);
                        break;
                    }
                }

                Fight.Move(this, result.Cells.Select(entry => entry.Point.CellId).ToArray());

                UseMP((short)(result.MPCost));
                MpUsedMove += result.MPCost;
            }

            CellMap lastCell = result.Cells.LastOrDefault();
            if (lastCell != null)
                Point.Point.CellId = (short)result.Cells.Last().Id;


            Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE, false);
        }
        public void MovementRequest(AmaknaCore.Debug.Utilities.Pathfinder.GamePathfinder.Pathfinder path, bool isMonster = false)
        {
            PreviousPosition = Point.Point.Clone() as MapPoint; ;
            var NextCells = new List<CellMap>();
            if (Stats.MP.Total < path.Cells.Count)
                return;

            List<Fighter> Tacklers = Fight.Fighters.FindAll(x => x.Point.Point.IsAdjacentTo(Point.Point) && x.Team.Team != Team.Team && x.IsAlive && !x.IsInvisible && !x.HasState(96));
            if (Tacklers.Count > 0 && !HasState(96) && !IsInvisible)
            {
                var tackedMp = GetTacledMP(Tacklers);
                var tackedAP = GetTackledAP(Tacklers);
                if (tackedMp > 0 || tackedAP > 0)
                {

                    Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                    if (tackedMp > 0 || tackedAP > 0)
                        Fight.Tackle(this, Tacklers.Select(entry => (double)entry.Id).ToArray());

                    if (tackedMp > 0)
                        UseMP((short)tackedMp, true);

                    if (tackedAP > 0)
                        UseAP((short)tackedAP, true);

                    for (int i = 1; i < path.Cells.Count(); i++)
                    {
                        if (Fight.HasTrap(path.Cells[i].Id) || Fight.MarkManager.HasPortail((short)path.Cells[i].Id) || Fight.HasGlyph(path.Cells[i].Id) || Fight.MarkManager.HasWall((short)path.Cells[i].Id))
                        {
                            path.CutPath(i + 1);
                            break;
                        }
                    }

                    Fight.Move(this, path.Cells.Select(entry => (short)entry.Id).ToArray());

                    UseMP((short)(path.Cells.Count));
                    MpUsedMove += path.Cells.Count;
                }
                else
                {
                    Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                    for (int i = 1; i < path.Cells.Count(); i++)
                    {
                        if (Fight.HasTrap(path.Cells[i].Id) || Fight.MarkManager.HasPortail((short)path.Cells[i].Id) || Fight.HasGlyph(path.Cells[i].Id) || Fight.MarkManager.HasWall((short)path.Cells[i].Id))
                        {
                            path.CutPath(i + 1);
                            break;
                        }
                    }
                    Fight.Move(this, path.Cells.Select(entry => (short)entry.Id).ToArray());
                    UseMP((short)(path.Cells.Count));
                    MpUsedMove += path.Cells.Count;
                }

            }
            else
            {
                Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                for (int i = 1; i < path.Cells.Count(); i++)
                {
                    if (Fight.HasTrap(path.Cells[i].Id) || Fight.MarkManager.HasPortail((short)path.Cells[i].Id) || Fight.HasGlyph(path.Cells[i].Id) || Fight.MarkManager.HasWall((short)path.Cells[i].Id))
                    {
                        path.CutPath(i + 1);
                        break;
                    }
                }
                Fight.Move(this, path.Cells.Select(entry => (short)entry.Id).ToArray());
                UseMP((short)(path.Cells.Count));
                MpUsedMove += path.Cells.Count;
            }

            var lastCell = path.Cells.LastOrDefault();
            if (lastCell != null)
            {
                UpdateAuraBuff(Point.Point.CellId);
                Point.Point.CellId = (short)lastCell.Id;
            }

            var portail = Fight.MarkManager.GetEndPortailByAnkama(Point.Point.CellId);
            if (portail != null)
            {
                portail.Item1.Execute(this);
                portail.Item2.Execute(this);
                ApplyTriggerBuff(TriggerBuffType.PORTAL, null);
                TeleportTo(this, (short)portail.Item2.TargetedCell.Id, ActionsEnum.ACTION_CHARACTER_TELEPORT_ON_SAME_MAP);
            }

            Fight.MarkManager.CheckReactive();

            Fight.MarkManager.ExecuteGlyph(this, TriggerBuffType.AURA);
            Fight.DeclancheTrap(lastCell.Id);
            Fight.MarkManager.DeclancheWall(this, true);

            Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE, false);

            if (NextCells.Count > 0)
            {
                NextCells.Insert(0, Fight.Map.Cells[path.Cells.Last().Id]);
                NextMove(NextCells);
            }
        }
        #endregion

        #region  Tackle
        public double GetTackledPourcent(Fighter fighter)
        {
            return fighter.Stats[PlayerFields.TackleBlock].Total != -2 ? (double)(this.Stats[PlayerFields.TackleEvade].Total + 2) / (2.0 * (double)(fighter.Stats[PlayerFields.TackleBlock].Total + 2)) : 0.0;
        }
        #endregion

        #region Event
        public void ActionPass()
        {
            if (Fight.TimeLine?.FighterPlaying.Id == Id)
                Fight.PassTurn();
        }
        public void PassTurn()
        {
            Fight.StartSequence(SequenceTypeEnum.SEQUENCE_TURN_END);
            ApplyTriggerBuff(TriggerBuffType.TURN_END, null);
            Fight.MarkManager.ExecuteGlyph(this, TriggerBuffType.TURN_END);
            Fight.MarkManager.DeclancheWall(this, true);
            Fight.MarkManager.ResetWallFighter(this);
            Fighter nextFighter = Fight.TimeLine.GetNextFighter();
            if (nextFighter is SummonMonster && nextFighter.Controller())
            {
                Fight.SwitchController(nextFighter as SummonMonster);
            }
            else if (Control)
            {
                Fight.ResetController((this as SummonMonster));
            }
            List<Buff> buffs = (from x in BuffList
                                where x.Duration > 0 && (x is DelayedBuff && (x as DelayedBuff).IsTurnEnd || x.Caster.Id != Id)
                                select x).ToList();
            foreach (Buff item in buffs)
            {
                if (item.DecrementDuration())
                    RemoveAndDispellBuff(item);
            }
            Fight.EndSequence(SequenceTypeEnum.SEQUENCE_TURN_END, false);
            Fight.VerifChall(ChallengeActionEnum.PassTurn, this);
            Stats.AP.Used = 0;
            Stats.MP.Used = 0;
            Control = false;
            SpellHistory.PassTurn();
        }
        public void PassTurnEnd()
        {
            Stats.AP.Used = 0;
            Stats.MP.Used = 0;
            Control = false;
        }
        public void OnTurnStarted()
        {
            Fight.StartSequence(SequenceTypeEnum.SEQUENCE_TURN_START);   
            foreach (var item in Fight.MarkManager.Portails.FindAll(x => x.Caster.Id == Id && x.Neutral))
                item.Neutral = false;
            Fight.MarkManager.CheckReactive();
            Fight.MarkManager.DeclancheWall(this, true);
            SkipTurn = BuffList.Any(x => x is SkipTurnBuff);
            //foreach (var buff in BuffList.FindAll(x => x is DelayedBuff))
            //{
            //    buff.Apply();
            //}
            UpdateAuraBuff(Point.Point.CellId);
            ApplyTriggerBuff(TriggerBuffType.TURN_BEGIN, null);
            Fight.MarkManager.DrecrementGlyph(this);
            Fight.MarkManager.ExecuteGlyph(this, TriggerBuffType.TURN_BEGIN);
            Fight.MarkManager.ExecuteGlyph(this, TriggerBuffType.AURA);
            List<Buff> buffs = (from x in BuffList
                                where x.Duration > 0 && x.Caster.Id == Id && (!(x is DelayedBuff) || (x is DelayedBuff && !(x as DelayedBuff).IsTurnEnd))
                                select x).ToList();
            buffs.AddRange((from x in Summons
                            where x is SummonBomb
                            from y in x.BuffList
                            //where y is DelayedBuff
                            select y));
            foreach (Buff item in buffs)
            {
                if (item.DecrementDuration())
                    item.Target.RemoveAndDispellBuff(item);
            }
            foreach (SummonIllu item in Summons.FindAll(x => x is SummonIllu))
            {
                item.Die(this);
            }
            Fight.EndSequence(SequenceTypeEnum.SEQUENCE_TURN_START, false);
            StartTurnPoint = Point.Point.Clone() as MapPoint;
            Fight.CheckFightEnd();
            if (this is CharacterFighter)
                (Fight.TimeLine.FighterPlaying as CharacterFighter).Character.RefreshStats();
            if (IsAlive)
            {
                if (SkipTurn)
                {
                    Fight.PassTurn();
                }
                else if (IsDisconnected)
                {
                    if (Fight.TimeLine.ActualRound - TurnDisconnect >= TurnBeforeDeco)
                    {
                        IsExclu = true;
                        Fight.RemoveCharacter(this);
                        //WorldManager.Instance.RemoveDisconnect((this as CharacterFighter).Character);
                    }
                    else
                    {
                        Fight.SendToAllCharacter(new TextInformationMessage(0, 162, new string[] { (this as CharacterFighter).Character.Infos.Name, (TurnBeforeDeco - (Fight.TimeLine.ActualRound - TurnDisconnect)).ToString() }));
                        Fight.PassTurn();
                    }
                }
                else
                {
                    if (this is AbstractIA)
                        (this as AbstractIA).Play();
                }
            }
        }
        public void UseMP(short cost, bool lost = false)
        {
            Stats.MP.Used += cost;
            Fight.PointsVariation(this, this, lost ? ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_LOST : ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE, cost * -1);
        }
        public void UseAP(short cost, bool lost = false)
        {
            Stats.AP.Used += cost;
            Fight.PointsVariation(this, this, lost ? ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_LOST : ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE, cost * -1);
        }
        public void RegainAP(short count)
        {
            Stats.AP.Used -= count;
            Fight.PointsVariation(this, this, Common.Protocol.Enums.ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_WIN, count);
        }
        public void RegainMP(short count)
        {
            Stats.MP.Used -= count;
            Fight.PointsVariation(this, this, Common.Protocol.Enums.ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_WIN, count);
        }
        public virtual void Die(Fighter by)
        {
            if(!IsDead)
            {
                IsDead = true;
                ApplyTriggerBuff(TriggerBuffType.DIE, null);
                foreach (var item in EffectsDie)
                {
                    item.TargetedPoint = Point.Point;
                    item.TargetedCell = Fight.Map.Cells[Point.Point.CellId];
                    item.Apply(null, true);
                }
                EffectsDie.Clear();
                RemoveAllBuff();
                if (Stats.Health.Total > 0)
                {
                    Stats.Health.DamageTaken = Stats.Health.TotalMax;
                }
                Fight.Die(by, this);
                var newSummons = (from x in Summons
                                  select x).ToList();
                KillAllSummon();
                Fight.RemoveAllBuff(this);
                Fight.MarkManager.RemovePortail(this);
                if (Fight.IsStarting && !Fight.IsEnded && Fight.TimeLine.FighterPlaying == this)
                    Fight.PassTurn();
                Fight.VerifChall(ChallengeActionEnum.Die, this);
            }
        }
        #endregion

        #region Summon
        public void AddSummon(SummonMonster summon)
        {
            this.Summons.Add(summon);
            Team.AddFighter(summon);
            Fight.Fighters.Insert(Fight.Fighters.IndexOf(this) + 1, summon);
            Fight.SummonAdded(summon);
            Fight.DeclancheTrap(summon.Point.Point.CellId);
        }
        public void AddStaticSummon(SummonMonster summon)
        {
            summon.IsStatic = true;
            this.Summons.Add(summon);
            Team.AddFighter(summon);
            Fight.Fighters.Add(summon);
            Fight.SummonAdded(summon);
            Fight.DeclancheTrap(summon.Point.Point.CellId);
        }
        public void KillAllSummon()
        {
            var newSummons = (from x in Summons
                              select x).ToList();
            foreach (var summon in newSummons)
                summon.Die(this);
        }
        public int CountBomb()
        {
            return Summons.Count(x => x is SummonBomb);
        }
        public List<SummonMonster> GetBombActivate(Fighter isMe)
        {
            var sums = Summons.FindAll(x => x is SummonBomb && (x as SummonBomb).IsActive && x.Id != isMe.Id);
            return sums;
        }
        public void ResetBombBonus()
        {
            if (!Summons.FindAll(x => x is SummonBomb).Any(x => x.IsAlive && (x as SummonBomb).IsActive))
                Stats[PlayerFields.BombBonus].Context = 0;
        }
        #endregion

        #region "Spell Zone"
        public void TryCastSpell(SpellTemplate spell, short cellId, bool isMonster = false)
        {
            if (spell.Spell != null && spell.Spell.id == 0)
            {
                var item = (this as CharacterFighter).Character.Inventory.GetItemByPosition(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON);
                if (item != null)
                {
                    Weapon weapon = item.Template as Weapon;
                    Fight.StartSequence(SequenceTypeEnum.SEQUENCE_WEAPON);
                    var goodEffects = (from x in weapon.possibleEffects
                                       where EffectManager.Instance.IsUnRandomableWeaponEffect((EffectsEnum)x.effectId)
                                       select x as EffectInstanceDice).ToList();
                    spell = new SpellTemplate(new Spell(), 0, new SpellLevel()
                    {
                        apCost = (uint)weapon.apCost,
                        castInDiagonal = weapon.castInDiagonal,
                        castInLine = weapon.castInLine,
                        castTestLos = weapon.castTestLos,
                        criticalHitProbability = (uint)weapon.criticalHitProbability,
                        range = (uint)weapon.range,
                        minRange = (uint)weapon.minRange,
                        rangeCanBeBoosted = false,
                        globalCooldown = 0,
                        grade = 1,
                        effects = goodEffects,
                        criticalEffect = goodEffects,
                        spellId = (uint)weapon.id,
                        maxCastPerTurn = weapon.maxCastPerTurn,
                        statesRequired = new List<int>(),
                        statesForbidden = new List<int>() { 42 }
                    })
                    {
                        IsWeapon = true,
                        CritAddDamge = weapon.criticalHitBonus / goodEffects.Count,
                    };
                    foreach (var cea in spell.CurrentSpellLevel.effects)
                    {
                        cea.targetMask = "g,G";
                        switch (weapon.typeId)
                        {
                            case 4:
                                cea.rawZone = "T1";
                                break;
                            case 7:
                                cea.rawZone = "C1";
                                break;
                            case 8:
                                cea.rawZone = "L1";
                                break;
                            default:
                                cea.rawZone = "P1";
                                break;
                        }
                    }
                    if (CanCastSpell(spell, cellId, null))
                    {
                        Fighter target = Fight.GetFighter(Fight.Map.Cells[cellId]);
                        if (target != null)
                            SpellHistory.UsedSpell(spell, target.Id);
                        else
                            SpellHistory.UsedSpell(spell, 0);
                        FightSpellCastCriticalEnum critical = GetIsCritical(spell);
                        SpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(this, spell, Fight.Map.Cells[cellId], critical == FightSpellCastCriticalEnum.CRITICAL_HIT ? true : false);
                        spellCastHandler.Initialize(true);
                        if (IsInvisible && !IsInvisibleSpellCast(spell))
                            Reveal();
                        Fight.CastedWeapon(this, spellCastHandler, Fight.Map.Cells[cellId], critical, false);
                        this.UseAP((short)spell.CurrentSpellLevel.apCost);
                        ApUseActions += (int)spell.CurrentSpellLevel.apCost;
                        spellCastHandler.Execute(null);
                        Fight.EndSequence(SequenceTypeEnum.SEQUENCE_WEAPON, false);
                        /// Check end
                        Fight.CheckFightEnd();
                    }
                    else
                    {
                        Fight.NoCastSpell(this, spell.CurrentSpellLevel.id);
                        Fight.EndSequence(SequenceTypeEnum.SEQUENCE_WEAPON, false);
                    }
                }
                else
                {
                    Fight.StartSequence(SequenceTypeEnum.SEQUENCE_WEAPON);
                    if (CanCastSpell(spell, cellId, null))
                    {
                        Fighter target = Fight.GetFighter(Fight.Map.Cells[cellId]);
                        if (target != null)
                            SpellHistory.UsedSpell(spell, target.Id);
                        else
                            SpellHistory.UsedSpell(spell, 0);
                        FightSpellCastCriticalEnum critical = GetIsCritical(spell);
                        SpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(this, spell, Fight.Map.Cells[cellId], critical == FightSpellCastCriticalEnum.CRITICAL_HIT ? true : false);
                        spellCastHandler.Initialize(true);
                        if (IsInvisible && !IsInvisibleSpellCast(spell))
                            Reveal();
                        this.UseAP((short)spell.CurrentSpellLevel.apCost);
                        this.OnSpellCasting(spellCastHandler, Fight.Map.Cells[cellId], critical, spellCastHandler.SilentCast);
                        ApUseActions += (int)spell.CurrentSpellLevel.apCost;
                        if (spell.CurrentSpellLevel.needTakenCell && target == null)
                        {
                            Fight.CheckFightEnd();
                            Fight.EndSequence(SequenceTypeEnum.SEQUENCE_WEAPON, false);
                        }
                        else
                        {
                            spellCastHandler.Execute(null);

                            Fight.CheckFightEnd();
                            Fight.EndSequence(SequenceTypeEnum.SEQUENCE_WEAPON, false);
                        }
                    }
                    else
                    {
                        Fight.NoCastSpell(this, spell.CurrentSpellLevel.id);
                        Fight.EndSequence(SequenceTypeEnum.SEQUENCE_WEAPON, false);
                    }
                }
            }
            else
            {
                Portail end = null;
                Portail start = null;
                if (Fight.MarkManager.HasPortail(cellId) && spell.CurrentSpellLevel.spellId != 5381)
                {
                    var two = Fight.MarkManager.GetEndPortailByAnkama(cellId);
                    start = two.Item1;
                    end = two.Item2;
                    if (end != null)
                    {
                        var syme = Point.Point.GetCellSymetrieByPortail(new MapPoint(cellId), new MapPoint(end.TargetedCell));
                        cellId = syme.CellId;
                    }
                }
                Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);
                if (CanCastSpell(spell, cellId, end))
                {
                    Fighter target = Fight.GetFighter(Fight.Map.Cells[cellId]);
                    if (target != null)
                        SpellHistory.UsedSpell(spell, target.Id);
                    else
                        SpellHistory.UsedSpell(spell, 0);
                    FightSpellCastCriticalEnum critical = GetIsCritical(spell);
                    SpellCastHandler spellCastHandler = SpellManager.Instance.GetSpellCastHandler(this, spell, Fight.Map.Cells[cellId], critical == FightSpellCastCriticalEnum.CRITICAL_HIT ? true : false);
                    spellCastHandler.Portails = (start != null ? new short[] { start.Id, end.Id } : new short[0]);
                    spellCastHandler.BoostCase = (start != null ? (1d + ((double)start.Spell.CurrentSpellLevel.effects[0].value + 2d * (double)Fight.MarkManager.GetCasesCount((short)start.TargetedCell.Id)) / 100d) : 0);
                    spellCastHandler.Initialize(true);
                    if (IsInvisible && !IsInvisibleSpellCast(spell))
                        Reveal();
                    this.UseAP((short)spell.CurrentSpellLevel.apCost);
                    this.OnSpellCasting(spellCastHandler, Fight.Map.Cells[cellId], critical, spellCastHandler.SilentCast);
                    ApUseActions += (int)spell.CurrentSpellLevel.apCost;
                    if (spell.CurrentSpellLevel.needTakenCell && target == null)
                    {
                        Fight.CheckFightEnd();
                        Fight.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL, false);
                    }
                    else
                    {
                        spellCastHandler.Execute(null);

                        Fight.CheckFightEnd();
                        Fight.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL, false);
                    }
                }
                else
                {
                    Fight.NoCastSpell(this, spell.CurrentSpellLevel.id);
                    Fight.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL, false);
                }
            }
            Fight.FighterTeleport.Clear();
        }

        public FightSpellCastCriticalEnum GetIsCritical(SpellTemplate spell)
        {
            if (spell.CurrentSpellLevel.criticalHitProbability == 0)
                return FightSpellCastCriticalEnum.NORMAL;
            var total = spell.CurrentSpellLevel.criticalHitProbability + Stats[PlayerFields.CriticalHit].Total;
            var rdnResult = new Random().Next(0, 100);
            return total < rdnResult ? FightSpellCastCriticalEnum.NORMAL : FightSpellCastCriticalEnum.CRITICAL_HIT;
        }

        public bool CanCastSpell(SpellTemplate spell, short cellId, Portail endPortail)
        {
            var start = endPortail != null ? new MapPoint(endPortail.TargetedCell) : Point.Point;
            var point = new MapPoint(cellId);
            Fighter target = Fight.GetFighter(point);
            if (!SpellHistory.CanUseSpell(spell))
                return false;
            else
                if (target != null && !SpellHistory.CanUseSpellInTarget(spell, target.Id))
                return false;
            if (spell.CurrentSpellLevel.needFreeCell && target != null)
                return false;
            if (spell.CurrentSpellLevel.needFreeTrapCell && Fight.HasCenterTrap(cellId))
                return false;
            if (Stats.AP.Total < spell.CurrentSpellLevel.apCost)
                return false;
            if (spell.CurrentSpellLevel.statesRequired.Count > 0)
            {
                var count = (from entry in (from x in BuffList where x is StateBuff select x as StateBuff)
                             where spell.CurrentSpellLevel.statesRequired.Contains(entry.State.id)
                             select entry).Count();
                if (count != spell.CurrentSpellLevel.statesRequired.Count)
                    return false;
            }
            if (spell.CurrentSpellLevel.statesForbidden.Count > 0)
            {
                foreach (var item in from x in BuffList where x is StateBuff select x as StateBuff)
                {
                    if (spell.CurrentSpellLevel.statesForbidden.Contains(item.State.id))
                        return false;
                }
            }
            var distance = point.DistanceToCell(start);
            var spellBoost = Stats.SpellBoosts.GetSpellBoost((int)spell.CurrentSpellLevel.spellId, CharacterSpellModificationTypeEnum.RANGE);
            var totalRange = 0;
            totalRange = (int)(spell.CurrentSpellLevel.range + (spell.CurrentSpellLevel.rangeCanBeBoosted ? Stats[PlayerFields.Range].Total + spellBoost : 0 + spellBoost));
            if (spell.CurrentSpellLevel.castInLine && spell.CurrentSpellLevel.castInDiagonal)
            {
                if ((distance > (totalRange < 1 ? 1 : totalRange) && distance / 2 > (totalRange < 1 ? 1 : totalRange)))
                {
                    return false;
                }
            }
            else if (spell.CurrentSpellLevel.castInLine)
            {
                if (distance > (totalRange < 1 ? 1 : totalRange) || distance < spell.CurrentSpellLevel.minRange)
                {
                    return false;
                }
                if (!start.IsLine(point))
                {
                    return false;
                }
            }
            else if (spell.CurrentSpellLevel.castInDiagonal)
            {
                if (distance / 2 > (totalRange < 1 ? 1 : totalRange) || distance / 2 < spell.CurrentSpellLevel.minRange)
                {
                    return false;
                }
                if (!start.IsDiagonale(point))
                {
                    return false;
                }
            }
            else
            {
                if (distance > (totalRange < 1 ? 1 : totalRange) || distance < spell.CurrentSpellLevel.minRange)
                {
                    return false;
                }
            }
            if (spell.CurrentSpellLevel.castTestLos)
            {
                List<MapPoint> cells = (from x in start.GetCellsInLine(point)
                                        where !Fight.Map.Cells[x.CellId].Los || (Fight.GetFighter(x) != null && Fight.GetFighter(x).IsAlive)
                                        select x).ToList();
                if (spell.CurrentSpellLevel.castTestLos && cells.Count - 2 > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void OnSpellCasted(SpellTemplate spell, CellMap targetCell, FightSpellCastCriticalEnum critical, bool silentCast)
        {
        }

        private void OnSpellCasting(SpellCastHandler spell, CellMap targetCell, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            if (spell.GetEffectHandlers().Any(x => x.Effect.EffectId == EffectsEnum.Effect_Trap))
                Fight.CastedSpell(this, spell, Fight.Map.Cells[spell.Caster.Point.Point.GetNearestCellInDirection(spell.Caster.Point.Point.OrientationTo(new MapPoint(targetCell))).CellId], critical, silentCast);
            else
                Fight.CastedSpell(this, spell, targetCell, critical, silentCast);
        }

        public bool IsInvisibleSpellCast(SpellTemplate spell)
        {
            SpellLevel currentSpellLevel = spell.CurrentSpellLevel;
            return (currentSpellLevel.effects.Any(entry => entry.effectId == (uint)EffectsEnum.Effect_Trap));
        }
        #endregion

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
