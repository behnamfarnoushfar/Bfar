using System.Collections;

namespace Bfar.XCutting.Foundation.Extensions
{
    public static class BitArrayExtensions
    {
        public static void Increment(this BitArray array, int blockSize = 4)
        {
            for (int block = blockSize; block >= 0; block--)
            {
                int lsb = block * 8 - 8;
                for (int i = lsb; i < block * 8; i++)
                {
                    if (!array[i])
                    {
                        array[i] = true;
                        if (blockSize - block > 0)
                        {
                            if (i > lsb)
                                for (int j = i - 1; j >= lsb; j--)
                                {
                                    array[j] = false;
                                }
                            for (int j = blockSize * 8 - 1; j >= lsb + 8; j--)
                            {
                                array[j] = false;
                            }
                            block++;
                        }
                        else if (i > lsb)
                            for (int j = i - 1; j >= lsb; j--)
                            {
                                array[j] = false;
                            }
                        return;
                    }
                }
            }
        }
    }
}
