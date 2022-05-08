package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseTrash;
import utility.DataReader;
import core.NetworkManager;

public class RequestTrash extends GameRequest {
    private int x, y;
    // Responses
    private ResponseTrash responseTrash;

    public RequestTrash() {
        responses.add(responseTrash = new ResponseTrash());
    }

    @Override
    public void parse() throws IOException {
        x = DataReader.readInt(dataInput);
        y = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responseTrash.setPlayer(player);
        responseTrash.setData(x, y);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseTrash);
    }
}