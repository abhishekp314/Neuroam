#include <PrecompiledHeader.h>

#include <QueryHandler.h>

int main(int argc, char** argv)
{
    if (argc > 0)
    {
        Neuroam::QueryHandler queryHandler(true);
        queryHandler.OnSearchChanged(argv[1]);
        queryHandler.OnFinalizeQuery();
    }
    return 0;
}