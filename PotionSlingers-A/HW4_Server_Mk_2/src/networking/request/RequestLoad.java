package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseLoad;
import utility.DataReader;
import core.NetworkManager;

public class RequestLoad extends GameRequest {
    private int x, y;
    // Responses
    private ResponseLoad responseLoad;

    public RequestLoad() {
        responses.add(responseLoad = new ResponseLoad());
    }

    @Override
    public void parse() throws IOException {
        x = DataReader.readInt(dataInput);
        y = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responseLoad.setPlayer(player);
        responseLoad.setData(x, y);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseLoad);
    }
}