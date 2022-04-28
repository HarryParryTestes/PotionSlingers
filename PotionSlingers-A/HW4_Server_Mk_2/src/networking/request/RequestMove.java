package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseMove;
import utility.DataReader;
import core.NetworkManager;

public class RequestMove extends GameRequest {
    private int x, y, z;
    // Responses
    private ResponseMove responseMove;

    public RequestMove() {
        responses.add(responseMove = new ResponseMove());
    }

    @Override
    public void parse() throws IOException {
        x = DataReader.readInt(dataInput);
        y = DataReader.readInt(dataInput);
        z = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responseMove.setPlayer(player);
        responseMove.setData(x, y, z);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseMove);
    }
}