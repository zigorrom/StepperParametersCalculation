using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepperParametersCalculation
{
    public class StepperCalculation
    {
        private int _colNum;
        private int _rowNum;
        private double _cellWidth;//in millimeters
        private double _stageLengthPerStep;
        private double _rotatorAnglePerStep;
        private double _stageCenterToDispencerLength;
        private int _stepsFromCenterToDispencer;
        private double _midColIndex;
        private double _midRowIndex;


        //optimize - 4 time same info
        private int[,] StageSteps;

        private int[,] AngleSteps;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ColNum"> Number of columns</param>
        /// <param name="RowNum">Number of rows</param>
        /// <param name="CellWidth">Width of single cell in millimeters </param>
        /// <param name="StageLengthPerStep">Length which stage moves per single step</param>
        /// <param name="RotatorAnglePerStep">Angle which stage rotates per single step</param>
        /// <param name="StageCenterToDispencerLength">Default home distance between stage center and dispencing position in millimeters</param>
        public StepperCalculation(int ColNum, int RowNum, double CellWidth, double StageLengthPerStep, double RotatorAnglePerStep, double StageCenterToDispencerLength)
        {
            _colNum = ColNum;
            _rowNum = RowNum;
            _cellWidth = CellWidth;
            _stageLengthPerStep = StageLengthPerStep;
            _rotatorAnglePerStep = RotatorAnglePerStep;
            _stageCenterToDispencerLength = StageCenterToDispencerLength;
            _midColIndex = (_colNum - 1.0) / 2;
            _midRowIndex = (_rowNum - 1.0) / 2;
            _stepsFromCenterToDispencer = (int)Math.Round(_stageCenterToDispencerLength / _stageLengthPerStep);

            StageSteps = new int[_rowNum, _colNum];
            AngleSteps = new int[_rowNum, _colNum];
            InitArrays();
        }



        private void InitArrays()
        {
            for (int i = 0; i < _rowNum; i++)
            {
                for (int j = 0; j < _colNum; j++)
                {
                    StageSteps[i, j] = (int)Math.Round(LengthFromCellToStageCenter(j, i) / _stageLengthPerStep);
                    AngleSteps[i, j] = (int)Math.Round(AngleFromCellToXaxis(j, i) / _rotatorAnglePerStep);
                }
            }
        }


        public int GetSteps(int Col, int Row)
        {
            if (Col < 0 || Col >= _colNum)
                return 0;
            if (Row < 0 || Row >= _rowNum)
                return 0;
            return _stepsFromCenterToDispencer - StageSteps[Row, Col];
        }

        public int GetAngles(int Col, int Row)
        {
            if (Col < 0 || Col >= _colNum)
                return 0;
            if (Row < 0 || Row >= _rowNum)
                return 0;
            return AngleSteps[Row, Col];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ColumnNumber">Zero-based column number</param>
        /// <param name="RowNumber">Zero-based row number</param>
        /// <returns>Length between zero-point of stage and current cell in millimeters </returns>

        public double LengthFromCellToStageCenter(int ColumnNumber, int RowNumber)
        {
            if (ColumnNumber >= _colNum || ColumnNumber < 0)
                return 0;
            if (RowNumber >= _rowNum || RowNumber < 0)
                return 0;
            var colIndexLength = Math.Abs(ColumnNumber - _midColIndex);
            var rowIndexLength = Math.Abs(RowNumber - _midRowIndex);
            var totalLength = _cellWidth * Math.Sqrt(colIndexLength * colIndexLength + rowIndexLength * rowIndexLength); // in millimeters
            return totalLength;
        }

        public double AngleFromCellToXaxis(int ColumnNumber, int RowNumber)
        {
            if (ColumnNumber >= _colNum || ColumnNumber < 0)
                return 0;
            if (RowNumber >= _rowNum || RowNumber < 0)
                return 0;
            var x = ColumnNumber - _midColIndex;
            var y = RowNumber - _midRowIndex;
            var angle = -1 * Math.Atan2(y, x) * (180 / Math.PI);
            if (angle < 0)
                angle = 360 + angle;
            return angle;
        }

        public int RotatorHalfCircleSteps
        {
            get { return (int)Math.Round(180 / _rotatorAnglePerStep); }
        }

        public int RotatorFullCircleSteps
        {
            get { return (int)Math.Round(360 / _rotatorAnglePerStep); }
        }


        public int StageMovementSteps(int ColumnNumber, int RowNumber, int PrevColumnNumber, int PrevRowNumber)
        {
            var prevXsteps = GetSteps(PrevColumnNumber,PrevRowNumber);
            var currXsteps = GetSteps(ColumnNumber, RowNumber);
            var stepsToMove = currXsteps - prevXsteps;
            return stepsToMove;
        }

        public int StageRotationSteps(int ColumnNumber, int RowNumber, int PrevColumnNumber, int PrevRowNumber)
        {
            var prevAngle = GetAngles( PrevColumnNumber,PrevRowNumber);
            var currAngle = GetAngles(ColumnNumber, RowNumber);
            var stepToMove = currAngle - prevAngle;
            if (stepToMove > RotatorHalfCircleSteps)
                stepToMove = stepToMove - RotatorFullCircleSteps;
            return stepToMove;

        }

    }
}
