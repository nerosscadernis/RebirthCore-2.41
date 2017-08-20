using Rebirth.Common.Extensions;
using Rebirth.Common.Protocol.Enums;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Game.Fights.Actors;
using Rebirth.World.Game.Zones.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebirth.World.Game.Zones
{
    public class Zone : IShape
    {
        private IShape m_shape;

        private SpellShapeEnum m_shapeType;

        private byte m_radius;

        private DirectionsEnum m_direction;

        public SpellShapeEnum ShapeType
        {
            get
            {
                return this.m_shapeType;
            }
            set
            {
                this.m_shapeType = value;
                this.InitializeShape();
            }
        }

        public IShape Shape
        {
            get
            {
                return this.m_shape;
            }
        }

        public uint Surface
        {
            get
            {
                return this.m_shape.Surface;
            }
        }

        public byte MinRadius
        {
            get;
            set;
        }

        public DirectionsEnum Direction
        {
            get
            {
                return this.m_direction;
            }
            set
            {
                this.m_direction = value;
                if (this.m_shape != null)
                {
                    this.m_shape.Direction = value;
                }
            }
        }

        public byte Radius
        {
            get
            {
                return this.m_radius;
            }
            set
            {
                this.m_radius = value;
                if (this.m_shape != null)
                {
                    this.m_shape.Radius = value;
                }
            }
        }

        public byte Size { get; set; }

        public Zone(SpellShapeEnum shape, byte radius, byte minRadius)
        {
            MinRadius = minRadius;
            this.Radius = radius;
            this.ShapeType = shape;
        }

        public Zone(SpellShapeEnum shape, byte radius, byte minRadius, DirectionsEnum direction)
        {
            MinRadius = minRadius;
            if (shape == SpellShapeEnum.L)
            {
                this.Size = radius;
            }
            else
            {
                this.Radius = radius;
            }
            this.Direction = direction;
            this.ShapeType = shape;
        }

        public Fighter Fighter
        {
            get;
            set;
        }

        public bool StopAtTarget
        {
            get;
            set;
        }

        public bool FromCaster
        {
            get;
            set;
        }

        public int CasterCellId
        {
            get;
            set;
        }

        public Zone(SpellShapeEnum shape, byte radius, byte minRadius, DirectionsEnum direction, Fighter fighter, bool stopAtTarget)
        {
            Fighter = fighter;
            StopAtTarget = stopAtTarget;
            MinRadius = minRadius;
            if (shape == SpellShapeEnum.L)
            {
                this.Size = radius;
            }
            else
            {
                this.Radius = radius;
            }
            this.Direction = direction;
            this.ShapeType = shape;
        }

        public CellMap[] GetCells(CellMap centerCell, MapTemplate map)
        {
            return this.m_shape.GetCells(centerCell, map);
        }

        private void InitializeShape()
        {
            SpellShapeEnum shapeType = this.ShapeType;
            if (shapeType <= SpellShapeEnum.plus)
            {
                if (shapeType == SpellShapeEnum.sharp)
                {
                    this.m_shape = new Cross(1, this.Radius)
                    {
                        Diagonal = true
                    };
                    goto IL_24F;
                }
                switch (shapeType)
                {
                    case SpellShapeEnum.star:
                        this.m_shape = new Cross(0, this.Radius)
                        {
                            AllDirections = true
                        };
                        goto IL_24F;
                    case SpellShapeEnum.plus:
                        this.m_shape = new Cross(0, this.Radius)
                        {
                            Diagonal = true
                        };
                        goto IL_24F;
                }
            }
            else
            {
                if (shapeType == SpellShapeEnum.slash)
                {
                    this.m_shape = new Line(this.Radius);
                    goto IL_24F;
                }
                if (shapeType == SpellShapeEnum.minus)
                {
                    this.m_shape = new Cross(0, Radius)
                    {
                        OnlyPerpendicular = true,
                        Diagonal = true
                    };
                    goto IL_24F;
                }
                switch (shapeType)
                {
                    case SpellShapeEnum.a:
                    case SpellShapeEnum.A:
                        this.m_shape = new Lozenge(MinRadius, 63);
                        goto IL_24F;
                    case SpellShapeEnum.C:
                        this.m_shape = new Lozenge(MinRadius, this.Radius);
                        goto IL_24F;
                    case SpellShapeEnum.D:
                        this.m_shape = new Cross(MinRadius, this.Radius);
                        goto IL_24F;
                    case SpellShapeEnum.I:
                        this.m_shape = new Lozenge(this.Radius, 63);
                        goto IL_24F;
                    case SpellShapeEnum.l:
                        m_shape = new Line(this.Radius);
                        m_shape.MinRadius = MinRadius;
                        m_shape.FromCaster = true;
                        m_shape.StopAtTarget = StopAtTarget;
                        m_shape.CasterCellId = Fighter.Point.Point.CellId;
                        m_shape.Direction = Direction.GetOpposedDirection();
                        goto IL_24F;
                    case SpellShapeEnum.L:
                        this.m_shape = new Line(this.Size);
                        goto IL_24F;
                    case SpellShapeEnum.O:
                        this.m_shape = new Lozenge((byte)(this.Radius), this.Radius);
                        goto IL_24F;
                    case SpellShapeEnum.P:
                        this.m_shape = new Shape.Single();
                        goto IL_24F;
                    case SpellShapeEnum.Q:
                        this.m_shape = new Cross(1, this.Radius);
                        goto IL_24F;
                    case SpellShapeEnum.T:
                        this.m_shape = new Cross(0, this.Radius)
                        {
                            OnlyPerpendicular = true
                        };
                        goto IL_24F;
                    case SpellShapeEnum.U:
                        this.m_shape = new HalfLozenge(MinRadius, this.Radius);
                        goto IL_24F;
                    case SpellShapeEnum.V:
                        this.m_shape = new Cone(MinRadius, this.Radius);
                        goto IL_24F;
                    case SpellShapeEnum.W:
                        this.m_shape = new Square(MinRadius, this.Radius)
                        {
                            DiagonalFree = true
                        };
                        goto IL_24F;
                    case SpellShapeEnum.G:
                        this.m_shape = new HalfSquare(MinRadius, this.Radius);
                        goto IL_24F;
                    case SpellShapeEnum.X:
                        this.m_shape = new Cross(MinRadius, this.Radius);
                        goto IL_24F;
                }
            }
            this.m_shape = new Cross(0, 0);
        IL_24F:
            this.m_shape.Direction = this.Direction;
        }
    }
}
