using SDL2;
using static SDL2.SDL;
using System;

namespace OriNoco
{
	/// <summary>
	/// A represetation for 2 by 2 Matrix
	/// </summary>
	public struct Matrix2x2 : IEquatable<Matrix2x2>
	{
		public float m11;
		public float m21;

		public float m12;
		public float m22;

		public float determinant
		{
			get
			{
				return (m11 * m22) - (m12 * m21);
			}
		}

		public Matrix2x2 inverse
		{
			get
			{
				return new Matrix2x2(m22, -m21, -m12, m11) * (1 / determinant);
			}
		}

		/// <summary>
		/// Creates matrix with the format of 
		/// [m11][m12]
		/// [m21][m22]
		/// </summary>
		public Matrix2x2(float m11, float m21, float m12, float m22)
		{
			this.m11 = m11;
			this.m21 = m21;
			this.m12 = m12;
			this.m22 = m22;
		}

		/// <summary>
		/// Creates matrix with the format of
		/// [m11, m12]
		/// [m21, m22]
		/// </summary>
		public Matrix2x2(Vector2 first, Vector2 second)
		{
			this.m11 = first.x;
			this.m12 = first.y;

			this.m21 = second.x;
			this.m22 = second.y;
		}

		/// <summary>
		/// Creates matrix that will be used for rotation transformation
		/// </summary>
		public Matrix2x2(float angle)
		{
			m11 = Mathf.Cos(angle * Mathf.Deg2Rad);
			m12 = -Mathf.Sin(angle * Mathf.Deg2Rad);
			m21 = Mathf.Sin(angle * Mathf.Deg2Rad);
			m22 = Mathf.Cos(angle * Mathf.Deg2Rad);
		}

		public float this[int row, int column]
		{
			get => this[row + column * 2];
			set => this[row + column * 2] = value;
		}

		public float this[int index]
		{
			get
			{
				float result;
				switch (index)
				{
					case 0:
						result = m11;
						break;
					case 1:
						result = m21;
						break;
					case 2:
						result = m12;
						break;
					case 3:
						result = m22;
						break;
					default:
						throw new IndexOutOfRangeException("Invalid matrix index!");
				}
				return result;
			}
			set
			{
				switch (index)
				{
					case 0:
						m11 = value;
						break;
					case 1:
						m21 = value;
						break;
					case 2:
						m12 = value;
						break;
					case 3:
						m22 = value;
						break;
					default:
						throw new IndexOutOfRangeException("Invalid matrix index!");
				}
			}
		}

		public Vector2 TransformPoint(Vector2 origin, Vector2 point) => (this * (point - origin)) + origin;
		public override int GetHashCode() => GetColumn(0).GetHashCode() ^ GetColumn(1).GetHashCode() << 2;
		public override bool Equals(object obj) => obj is Matrix2x2 && Equals((Matrix2x2)obj);
		public bool Equals(Matrix2x2 other) => GetColumn(0).Equals(other.GetColumn(0)) && GetColumn(1).Equals(other.GetColumn(1));
		public Vector2 GetColumn(int index)
		{
			Vector2 result;
			switch (index)
			{
				case 0:
					result = new Vector2(m11, m21);
					break;
				case 1:
					result = new Vector2(m12, m22);
					break;
				default:
					throw new IndexOutOfRangeException("Invalid column index!");
			}
			return result;
		}

		public Vector2 GetRow(int index)
		{
			Vector2 result;
			switch (index)
			{
				case 0:
					result = new Vector2(m11, m12);
					break;
				case 1:
					result = new Vector2(m21, m22);
					break;
				default:
					throw new IndexOutOfRangeException("Invalid column index!");
			}
			return result;
		}

		public static bool operator ==(Matrix2x2 left, Matrix2x2 right) => left.Equals(right);
		public static bool operator !=(Matrix2x2 left, Matrix2x2 right) => !left.Equals(right);
		public static Vector2 operator *(Matrix2x2 lhs, Vector2 vector)
		{
			Vector2 result = default;
			result.x = (lhs.m11 * vector.x) + (lhs.m12 * vector.y);
			result.y = (lhs.m21 * vector.x) + (lhs.m22 * vector.y);
			return result;
		}

		public static Matrix2x2 operator *(Matrix2x2 lhs, float val) => new Matrix2x2(lhs.m11 * val, lhs.m21 * val, lhs.m12 * val, lhs.m22 * val);
		public static Vector2 operator /(Matrix2x2 lhs, Vector2 vector)
		{
			Vector2 result = default;
			result.x = (lhs.m11 / vector.x) + (lhs.m12 / vector.y);
			result.y = (lhs.m21 / vector.x) + (lhs.m22 / vector.y);
			return result;
		}
	}
}