var data;

var JsonStatus = function () {
    this.data = {
        Types: "",

        UUID: "",
        
        player1: {
            UUID: "",  
            Pos_x: "",
            Pos_y: "",
            Pos_z: "",
        }
        ,
        player2: {
            UUID: "",
            Pos_x: "",
            Pos_y: "",
            Pos_z: "",
        }

    }

}

JsonStatus.prototype.getData = function () {
    return this.data;
}

module.exports = JsonStatus;