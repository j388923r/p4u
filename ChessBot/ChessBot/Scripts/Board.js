var boardView = React.createElement('div', null, "Chess Board");

var board = (function () {
    var that = this;

    that.boardPieces = new Array(8);
    for (var row = 0; row < that.boardPieces.length; row++) {
        that.boardPieces[row] = new Array(8);
    }

    that.setBoard = function (boardArray) {
        if (boardArray === undefined || boardArray.length !== 8 || boardArray[0].length !== 8)
            return;

        for (; ;) {

        }
    }

    return that;
})();

ReactDOM.render(boardView, document.body.board);