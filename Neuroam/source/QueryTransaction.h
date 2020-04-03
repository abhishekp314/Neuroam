#pragma once

namespace Neuroam
{
    class QueryTransaction
    {
    public:
        QueryTransaction(const std::vector<long>& ids)
            : m_WordIds(ids)
            , m_Weight(0.0f)
        {
        }

        const std::vector<long>& GetWordIds() const { return m_WordIds; }
        bool ContainsWordId(long id) const;

    private:
        std::vector<long> m_WordIds;
        float m_Weight = 0.0f;
    };

    using QueryTransactions = std::vector<QueryTransaction>;
}