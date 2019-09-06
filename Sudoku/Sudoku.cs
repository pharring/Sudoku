using System;
using System.Text;

namespace Sudoku
{
    public class Sudoku
    {
        private static readonly IndexRowColumnBlock[] s_map = new IndexRowColumnBlock[81];

        private readonly struct IndexRowColumnBlock
        {
            public readonly byte Index;
            public readonly byte Row;
            public readonly byte Col;
            public readonly byte Block;

            public IndexRowColumnBlock(byte index)
            {
                Index = index;
                Row = (byte)(index / 9);
                Col = (byte)(index % 9);
                Block = (byte)(3 * (Row / 3) + (Col / 3));
            }
        }

        static Sudoku()
        {
            for (byte i = 0; i < 81; i++)
            {
                s_map[i] = new IndexRowColumnBlock(i);
            }
        }

        private readonly byte[] _board = new byte[81];
        private readonly ushort[] _rowBits = new ushort[9];
        private readonly ushort[] _colBits = new ushort[9];
        private readonly ushort[] _blockBits = new ushort[9];

        public Sudoku(string puzzle)
        {
            if (puzzle.Length != 81)
            {
                throw new ArgumentException("puzzle must be exactly 81 chars");
            }

            for (int i = 0; i < 81; i++)
            {
                var ch = puzzle[i];
                if (ch >= '0' && ch <= '9')
                {
                    SetValue(s_map[i], (byte)(ch - '0'));
                }
            }
        }

        public bool Solve() => Solve(default);

        private bool Solve(int idx)
        {
            for (; idx < 81; idx++)
            {
                if (!IsOccupied(idx))
                {
                    ref var position = ref s_map[idx];
                    var available = Available(position);
                    if (available != 0)
                    {
                        for (byte trialValue = 1; trialValue <= 9; trialValue++)
                        {
                            available >>= 1;

                            if ((available & 1) != 0)
                            {
                                SetValue(position, trialValue);
                                if (Solve(idx + 1))
                                {
                                    return true;
                                }
                            }
                        }

                        SetValue(position, 0);
                    }

                    return false;
                }
            }

            // Solution found
            return true;
        }

        private void SetValue(in IndexRowColumnBlock position, byte val)
        {
            ref byte slot = ref _board[position.Index];
            var oldVal = slot;
            slot = val;

            if (oldVal != 0)
            {
                var clearMask = (ushort)~(1 << oldVal);
                _rowBits[position.Row] &= clearMask;
                _colBits[position.Col] &= clearMask;
                _blockBits[position.Block] &= clearMask;
            }

            if (val != 0)
            {
                var setMask = (ushort)(1 << val);
                _rowBits[position.Row] |= setMask;
                _colBits[position.Col] |= setMask;
                _blockBits[position.Block] |= setMask;
            }
        }

        private bool IsOccupied(int position) => _board[position] != 0;

        private int Available(in IndexRowColumnBlock position) => ~(_rowBits[position.Row] | _colBits[position.Col] | _blockBits[position.Block]);

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("╔═══╤═══╤═══╦═══╤═══╤═══╦═══╤═══╤═══╗");

            for (int row = 0; row < 9; row++)
            {
                sb.Append("║ ");
                for (int col = 0; col < 9; col++)
                {
                    var val = _board[row * 9  + col];
                    sb.Append(val == 0 ? '.' : (char)(val + '0'));

                    if (col == 2 || col == 5)
                    {
                        sb.Append(" ║ ");
                    }
                    else if (col == 8)
                    {
                        sb.AppendLine(" ║");
                    }
                    else
                    {
                        sb.Append(" │ ");
                    }
                }

                if (row == 2 || row == 5)
                {
                    sb.AppendLine("╠═══╪═══╪═══╬═══╪═══╪═══╬═══╪═══╪═══╣");
                }
                else if (row == 8)
                {
                    sb.AppendLine("╚═══╧═══╧═══╩═══╧═══╧═══╩═══╧═══╧═══╝");
                }
                else
                {
                    sb.AppendLine("╟───┼───┼───╫───┼───┼───╫───┼───┼───╢");
                }
            }

            return sb.ToString();
        }
    }
}
