using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flats
{
    class TableBuilder {
        private Table _table;
        public TableBuilder() {
            _table = new Table();
        }

        public TableBuilder AddLine(string title, string data) {
            _table.AddLine(title, data);
            return this;
        }

        public string Build() => _table.ToString();

    }

    class TableLine {
        public string Title { get; set; }
        public string Data { get; set; }
    }

    class Table {
        private List<TableLine> _lines;
        private int _titleColumnMinWidth = 0;
        private int _dataColumnMinWidth = 0;
        private static char hLineSymbol = '-';
        private static char vLineSymbol = '|';

        public Table() {
            _lines = new List<TableLine>();
        }

        public void AddLine(string title, string data) {
            if (title.Length > _titleColumnMinWidth)
                _titleColumnMinWidth = title.Length;
            if (data.Length > _dataColumnMinWidth)
                _dataColumnMinWidth = data.Length;
            _lines.Add(new TableLine { Title = title, Data = data });
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            var hLineSize = _titleColumnMinWidth + _dataColumnMinWidth + /* three separators */ 5;
            var horizline = string.Concat(Enumerable.Repeat(hLineSymbol, hLineSize));
            sb.Append(string.Concat(horizline, Environment.NewLine));
            foreach (var line in _lines) {
                var title = line.Title;
                var data = line.Data;
                sb.Append($"{vLineSymbol}" +
                    $"{title.PadRight(_titleColumnMinWidth)}" +
                    "   " +
                    $"{data.PadRight(_dataColumnMinWidth)}" +
                    $"{vLineSymbol}" +
                    $"{Environment.NewLine}");

            }
            sb.Append(string.Concat(horizline, Environment.NewLine));
            return sb.ToString();
        }
    }
}
